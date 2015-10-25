using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    //a unit is a collection of n * n squares which must be unique. Think of it as a column, row, or block
    class Unit
    {
        Square[] myMembers;
        bool isComplete, updateUnhandled;
        bool[][] valueTable;
        int n4, n2, n;
        public Unit(Square[] members)
        {
            myMembers = members;
            n2 = myMembers.Length;
            n = (int)Math.Sqrt(n2);
            n4 = n2 * n2;
            isComplete = checkCompletion();
            valueTable = new bool[n2][];
            updateUnhandled = false;
        }
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
        public Square[] operate()
        {
            setImpossibles();
            elimination();
            findNSum();
            
            return myMembers;
        }
        public void elimination()
        {//there is only one value in the unit that can satisfy a value
            int counter = 0, onlyIndex = -1 ;
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
                    possibleValueTable = getPossibleValueTable();// updates have been made. Reflect this.
                    //value = 0;
                }
            }
        }
        public void findNSum()
        {
            int targetN, sum, counter;
            bool[] values;
            List<int> impossibles;
            int[] arrayOfPossibilitiesNum;
            int[] arrayOfBinaryRepresentation;
            arrayOfBinaryRepresentation = getBinary();
            arrayOfPossibilitiesNum = getPossiblitiesNum();

            for (int index = 0; index < n2; index++)
            {//loop through all the squares in this unit
                impossibles = new List<int>();//create a new list of impossible numbers
                values = new bool[n2]; //clear this array which tracks similar combinations
                targetN = arrayOfPossibilitiesNum[index];
                sum = arrayOfBinaryRepresentation[index];
                counter = 0;
                for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                {
                    if (sum == arrayOfBinaryRepresentation[secondaryIndex])
                    {
                        counter++;
                        values[secondaryIndex] = true;
                    }
                }
                if (counter == targetN)
                {
                    bool[] possibleArr = myMembers[index].getPossibleValues();
                    for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                    {//this loop is the function that gives the impossible values of the index. The values that the other numbers cannot be now
                        if (possibleArr[secondaryIndex])
                        {
                            impossibles.Add(secondaryIndex);
                        }
                    }
                    for (int secondaryIndex = 0; secondaryIndex < n2; secondaryIndex++)
                    {
                        if (!values[secondaryIndex])
                        {//these are the values that are not identical
                            myMembers[secondaryIndex].impossibleValues(impossibles);//pass the possible values as impossible to the other squares
                        }
                    }
                }
            }


            //int targetN, binary, counter;
            //bool[] values;
            //List<int> impossibles;
            //int[] arrayOfPossibilitiesNum = new int[n2];
            //int[] arrayOfBinaryRepresentation = new int[n2];
            //for (int i = 0; i < n2; i++)
            //{
            //    arrayOfBinaryRepresentation[i] = myMembers[i].binaryCompare();
            //    arrayOfPossibilitiesNum[i] = myMembers[i].numberOfPossibilities();
            //}
            //for (int first = 0; first < n2; first++)
            //{
            //    values = new bool[n2];
            //    counter = 0;
            //    targetN = arrayOfPossibilitiesNum[first];
            //    if (targetN == 1)
            //    {
            //        continue;
            //    }
            //    binary = arrayOfBinaryRepresentation[first];
            //    for (int secondary = 0; secondary < n2; secondary++)
            //    {
            //        if (binary == arrayOfBinaryRepresentation[secondary])
            //        {
            //            counter++;
            //            values[secondary] = true;
            //        }
            //    }
            //    if (counter == targetN && counter > 1)
            //    {
            //        impossibles = new List<int>();
            //        for (int i = 0; i < n2; i++)
            //        {
            //            if ((myMembers[i].getPossibleValues())[i])
            //            {
            //                impossibles.Add(i);
            //            }
            //        }
            //        for (int i = 0; i < n2; i++)
            //        {
            //            if (!values[i])
            //            {
            //                myMembers[i].impossibleValues(impossibles);
            //            }
            //        }
            //    }
            //}
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
                    //updateUnhandled = true
            }
        }

    }
}
