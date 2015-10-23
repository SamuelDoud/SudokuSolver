﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Square
    {
        private int myValue, myRow, myColumn, n, n2, myBlock;
        public const int NULL_VALUE = -1;
        private bool[] possibleValues;
        public Square(int row, int column, int n)
        {
            myValue = NULL_VALUE;
            myRow = row;
            myColumn = column;
            this.n = n;
            n2 = n * n;
            myBlock = column / n + n * (row / n);
            possibleValues = new bool[n2];
            possibleValues = setAllInBoolArrayTo(possibleValues, true);
        }
        /*
        Set all members of  a passed boolean array to a value true or false
        */
        private bool[] setAllInBoolArrayTo(bool[] array, bool value)
        {
            int length = array.Length; // instead of calling array.length in the loop, make a variable outside
            for (int i = 0; i < length; i++)
            {
                array[i] = value; //set element i to value
            }
            return array; // return the array
        }
        public bool[] getPossibleValues()
        {
            return possibleValues;
        }
        public void impossibleValue(int value)
        {
            possibleValues[value] = false;
            //check to see if it is the only possible value
            if (myValue == Square.NULL_VALUE)
            onlyPossible();
        }
        /*
        Check to see if there is only one possible value in this square
        If so, set it to that
        */
        public void onlyPossible()
        {
            
            int counter = 0;
            int lastIndex = -1;
            for (int i = 0; i < n2 && counter < 2; i++)
            {
                if (possibleValues[i])
                {
                    counter++;
                    lastIndex = i;
                }
            }
            if (counter == 1)
            {
                setValue(lastIndex);
            }
        }
        public void setValue(int value)
        {
            if (value >= 0 && value < n * n)//check legality of value
            {
                myValue = value;
                possibleValues = setAllInBoolArrayTo(possibleValues, false); //all the others are no longer valid
                possibleValues[myValue] = true; //set the value to true
            }
        }
        public int getValue()
        {
            return myValue;
        }
        public int getColumn()
        {
            return myColumn;
        }
        public int getRow()
        {
            return myRow;
        }
        public int getBlock()
        {
            return myBlock;
        }
        public int getGroupingValue(int group)
        {
            if (group == UnitType.ROW)
            {
                return getRow();
            }
            if (group == UnitType.COLUMN)
            {
                return getColumn();
            }
            if (group == UnitType.BLOCK)
            {
                return getBlock();
            }
            return -1;
        }
        public override string ToString()
        {
            return "(" + myRow + ", " + myColumn + ", " + myBlock + ")" + " is " + (myValue + 1);
        }
    }
}