using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    //a unit is a collection of n * n squares which must be unique. Think of it as a column, row, or block
    class Unit
    {
        private bool unitHasBeenChanged;
        private Square[] myMembers;
        private bool isComplete;
        private bool[][] valueTable;
        private int n4, n2, n, maxSum;
        public Unit(Square[] members)
        {
            unitHasBeenChanged = false;
            myMembers = members;
            n2 = myMembers.Length;
            n = (int)Math.Sqrt(n2);
            n4 = n2 * n2;
            maxSum = (int)Math.Pow(2, n2 - 1);
            isComplete = checkCompletion();
            valueTable = new bool[n2][];
        }
        /**
            Update the value table off possible valeus as it may be different
        */
        private void updateValueTable()
        {
            for (int position = 0; position < n2; position++)
            {
                valueTable[position] = myMembers[position].getPossibleValues();
            }
        }
        public bool checkCompletion()
        {
            bool tempCompletion = true;
            foreach (Square s in myMembers)
            {
                if (s.getValue() == Square.NULL_VALUE)
                {
                    tempCompletion = false;
                    break;
                }
            }
            isComplete = tempCompletion;//using a temp so threading wont mess this up
            return isComplete;
        }
        /**
            The method to call the solving methods.
            Will continue iteration until no more changes can be made
        */
        public Square[] operate()
        {
            do//always need to operate once
            {
                updatesHandled();
                setImpossibles();
                elimination();
                findNSum();
                FindHiddenTwins(); //This is not working properly yet
            } while (updatesUnhandled());//there are no updates which haven't been recognized
            return myMembers;
        }
        public bool changeStatus()
        {
            return unitHasBeenChanged;
        }
        /**
            A square has been updated and the calculations should be rerun
        */
        private bool updatesUnhandled()
        {
            foreach (Square s in myMembers)
            {
                if (s.getUpdateStatus())// a square has been updated
                {
                    unitHasBeenChanged = true;
                    return true;//if one square has been updated it all the squares need to be reevaluated
                }
            }
            return false;
        }
        /**
            Any updates within a square have been recognized.
        */
        private void updatesHandled()
        {
            for (int position = 0; position < n2; position++)
            {
                myMembers[position].updateHandled();
            }
        }
        private void elimination()
        {//there is only one value in the unit that can satisfy a value
            int counter = 0, onlyIndex = -1;
            bool[][] possibleValueTable = getPossibleValueTable();
            for (int value = 0; value < n2; value++)
            {
                counter = 0;
                for (int position = 0; position < n2 && counter < 2; position++)
                {
                    if (possibleValueTable[position][value])
                    {
                        counter++;
                        onlyIndex = position;
                    }
                }
                if (counter == 1)
                {
                    myMembers[onlyIndex].setValue(value);
                    possibleValueTable = getPossibleValueTable();
                }
            }
        }

        /**
        This method is looking for what would be called "twins" or triples... etc.
        If two squares in a unit have possible values of 2 and 5 and only those values.
        No other square in that unit can have 2 and 5 as possible values.
        */
        public void findNSum()
        {
            //Declarations
            int targetN, sum, counter;
            bool[] values;//Used to save values that are found to be NSums
            List<int> impossibles;
            int[] arrayOfPossibilitiesNum;
            int[] arrayOfBinaryRepresentation;
            //The next two method calls are operating on all squares
            arrayOfBinaryRepresentation = getBinary();//returns a number between 0-2^(n2). Unique for every possible square combo
            arrayOfPossibilitiesNum = getPossiblitiesNum();//get the number of possible values on a square
            for (int index = 0; index < n2; index++)
            {//loop through all the squares in this unit
                impossibles = new List<int>();//create a new list of impossible numbers
                values = new bool[n2]; //clear this array which tracks similar combinations
                targetN = arrayOfPossibilitiesNum[index];//targetN takes the number of possiblilities on a square.
                sum = arrayOfBinaryRepresentation[index];//sum is the binary sum of the square
                counter = 0;//counter tracks the number of like squares. If counter is equal to targetN at the end, then a Nsum is found 
                for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                {
                    if (sum == arrayOfBinaryRepresentation[secondaryIndex])//the squares have similar characteristics!
                    {
                        counter++;//increment the counter
                        values[secondaryIndex] = true;
                    }
                }
                if (counter == targetN)//We have found a NSum
                {
                    bool[] possibleArr = myMembers[index].getPossibleValues();
                    for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                    {//this loop is the function that gives the impossible values of the index. The values that the other numbers cannot be now
                        if (possibleArr[secondaryIndex])
                        {
                            impossibles.Add(secondaryIndex);
                        }
                    }
                    impossibles = myMembers[index].GetPossibleValuesList();//get the impossible values from the original square
                    for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                    {
                        if (!values[secondaryIndex])//the value is not in the Nsum, give it the possible values of the NSums
                        {//these are the values that are not identical
                            myMembers[secondaryIndex].impossibleValues(impossibles);//pass the possible values as impossible to the other squares
                        }
                    }
                    arrayOfBinaryRepresentation = getBinary();//since the squares have changed we need to redo this!
                }
            }
        }
        /**
            Hidden twins are if two values only appear twice and on the same square in a puzzle.
            Therefore those two squares must contain those two values.
            Remove anyother possible values from those squares.
            TODO generalize this to n instead of twins
        */
        private void FindHiddenTwins()
        {
            int[] countOfValueTable = countOfPossibleValues();
            int[] positions;
            int counter;
            List<int> values;
            //this array takes the number of squares that have a certain possible value
            //useful for determining what values are worth searching
            for (int value = 0; value < n2; value++)
            {

                if (countOfValueTable[value] == 2)
                {
                    for (int otherValue = 0; otherValue < n2; otherValue++)
                    {
                        if (value != otherValue && countOfValueTable[otherValue] == 2)// the values are not the same and the other value has 2 spots
                        {
                            //Console.WriteLine("???");
                            positions = new int[2];
                            counter = 0;
                            //now search for the psoitions of the two values. If they are the same, then we have a twin hidden pair.
                            for (int position = 0; position < n2; position++)
                            {

                                if (valueTable[position][value] && valueTable[position][otherValue])
                                {//this position has both values
                                    positions[counter] = position;//this position is legal in the twin
                                    counter++;
                                }
                                //Console.WriteLine(counter);
                            }
                            if (counter == 2)//this is a twin hidden! because two positions fufill this requiremnet
                            {
                                if (myMembers[positions[0]].numberOfPossibilities() != 2 || myMembers[positions[1]].numberOfPossibilities() != 2)
                                {
                                    values = new List<int>();//reset the value array
                                    for (int impossibleValue = 0; impossibleValue < n2; impossibleValue++)
                                    {
                                        if (impossibleValue != value && impossibleValue != otherValue)//TODO generalize this!! The add all the values to the valeus array except the two hidden values
                                        {
                                            values.Add(impossibleValue);
                                        }
                                        else
                                        {
                                            //Console.Write((impossibleValue + 1) + ", ");
                                        }
                                    }
                                    //Console.Write("\t");
                                    foreach (int position in positions)//Go through each of the positions that met the requirement
                                    {
                                        myMembers[position].impossibleValues(values);
                                        //Console.Write(myMembers[position].getRow() + ", " + myMembers[position].getColumn() + "\t");
                                    }
                                   // Console.WriteLine("Hidden Twin!");
                                }
                            }
                        }
                    }
                }
            }

        }
        /**
            This array takes the number of squares that have a certain possible value
        */
        private int[] countOfPossibleValues()
        {
            updateValueTable();//we should make sure we are using a relevant value table!
            int[] countOfValueTable = new int[n2];

            for (int position = 0; position < n2; position++)//iterate through each square
            {
                for (int value = 0; value < n2; value++)//iterate through each possible value
                {
                    if (valueTable[position][value])//is the value at this position true?
                    {
                        countOfValueTable[value]++;//add one to the position in the array
                    }
                }
            }
            return countOfValueTable;
        }
        private int[] getBinary()
        {
            int[] arrayOfBinaryRepresentation = new int[n2];
            for (int index = 0; index < n2; index++)
            {
                arrayOfBinaryRepresentation[index] = myMembers[index].binaryCompare();
            }
            return arrayOfBinaryRepresentation;
        }
        private int[] getPossiblitiesNum()
        {
            int[] arrayOfPossibilitiesNum = new int[n2];
            for (int index = 0; index < n2; index++)
            {
                arrayOfPossibilitiesNum[index] = myMembers[index].numberOfPossibilities();
            }
            return arrayOfPossibilitiesNum;
        }
        public bool[][] getPossibleValueTable()
        {
            int counter = 0;
            bool[][] tempPossibleValues = new bool[n2][];
            foreach (Square s in myMembers)
            {
                tempPossibleValues[counter] = s.getPossibleValues();
                counter++;
            }
            return tempPossibleValues;

        }
        public void setImpossibles()
        {//take out the values from the unit from the possible values of the others in the unit
            int value;
            List<int> impossibles = new List<int>();
            for (int aSquareWithValue = 0; aSquareWithValue < n2; aSquareWithValue++)
            {
                value = myMembers[aSquareWithValue].getValue();
                if (value == Square.NULL_VALUE)
                {
                    continue;
                }
                impossibles.Add(value);
            }
            for (int squareWithOutAValue = 0; squareWithOutAValue < n2; squareWithOutAValue++)
            {
                if (myMembers[squareWithOutAValue].getValue() == Square.NULL_VALUE)//the square has no value
                {
                    myMembers[squareWithOutAValue].impossibleValues(impossibles);
                }
            }
        }
        /**
            Tryign to simplify the NSum method by adding a method which gets
            the numbers not in a square by using its binary sum
        */
        public List<int> findNumsNotInBinary(int binarySum)
        {
            List<int> values = new List<int>();
            binarySum = maxSum - binarySum;
            for (int i = (int)Math.Pow(2, n2 - 1); i >= 1 && binarySum > 0; i /= 2)
            {
                if (binarySum >= i)
                {
                    binarySum -= i;//decrement binary sum
                    values.Add((int)Math.Log(i, 2));//return the logarithim of the indexing varible with base 2
                }
            }
            return values;
        }
    }
}
