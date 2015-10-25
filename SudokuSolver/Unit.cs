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
            int targetN, binary, counter;
            bool[] values;
            List<int> impossibles;
            int[] arrayOfPossibilitiesNum = new int[n2];
            int[] arrayOfBinaryRepresentation = new int[n2];
            for (int i = 0; i < n2; i++)
            {
                arrayOfBinaryRepresentation[i] = myMembers[i].binaryCompare();
                arrayOfPossibilitiesNum[i] = myMembers[i].numberOfPossibilities();
            }
            for (int first = 0; first < n2; first++)
            {
                values = new bool[n2];
                counter = 0;
                targetN = arrayOfPossibilitiesNum[first];
                if (targetN == 1)
                {
                    continue;
                }
                binary = arrayOfBinaryRepresentation[first];
                for (int secondary = 0; secondary < n2; secondary++)
                {
                    if (binary == arrayOfBinaryRepresentation[secondary])
                    {
                        counter++;
                        values[secondary] = true;
                    }
                }
                if (counter == targetN && counter > 1)
                {
                    impossibles = new List<int>();
                    for (int i = 0; i < n2; i++)
                    {
                        if ((myMembers[i].getPossibleValues())[i])
                        {
                            impossibles.Add(i);
                        }
                    }
                    for (int i = 0; i < n2; i++)
                    {
                        if (!values[i])
                        {
                            myMembers[i].impossibleValues(impossibles);
                        }
                    }
                }
            }
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
