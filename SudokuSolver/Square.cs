using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    //Class that holds the all the information associated with a square on the board
    //The value is -1 if the square's value is unknown
    //Possible Values are stored as bools in an array
    //A value can be determined to not be possible by passing that particular value as an integer by itself or in a list
    //If there is only one value possible, set that value equal to that value
    class Square : IComparable
    {
        private int myValue, myRow, myColumn, n, n2, myBlock;//n is the complexity of the given puzzle. 
        //block is an organizational unit like row and column. It is the 3x3 square of squares that are outlined in Sudoku puzzles
        public const int NULL_VALUE = -1;//the no value
        private bool[] possibleValues;//array of possible values for the square. Set to size n^2
        bool updateUnhandled;
        public Square(int row, int column, int n)
        {
            myValue = NULL_VALUE;//default values
            myRow = row;
            myColumn = column;
            this.n = n;
            n2 = n * n;//n2 is the square of n. In terms of a square, this is the number of possible values
            myBlock = column / n + n * (row / n);//The formula to determine a "block"
            possibleValues = new bool[n2];
            possibleValues = setAllInBoolArrayTo(possibleValues, true);
            updateUnhandled = false;
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
        /*
        Returns all of the legal values
        */
        public bool[] getPossibleValues()
        {
            return possibleValues;
        }
        /*
            Set one value to impossible in the array of possible values
            Then check to see if it is the only possible value left
        */
        public void impossibleValue(int value)
        {
            if (myValue == NULL_VALUE)//only need to run this operation if the square has no value
            {
                if (possibleValues[value])//only run this if the value being set to not possible is currently possible
                {
                    possibleValues[value] = false;
                    updateUnhandled = true;
                    onlyPossible();
                }
            }
        }
        /*
            Instead of using a single integer, take a list of integers to set to impossible instead
        */
        public void impossibleValues(List<int> values)
        {
            if (myValue == NULL_VALUE)//only need to run this operation if the square has no value
            {
                foreach (int value in values)//iterate through each integer
                {
                    if (possibleValues[value])//only run this if the value being set to not possible is currently possible
                    {
                        possibleValues[value] = false;
                        updateUnhandled = true;
                        //the integer method is not called here because it checks for the onlyPossible on every call. A waste of resources.
                    }
                }
                onlyPossible();//check to see if only one value is legal
            }
        }
        /*
        Check to see if there is only one possible value in this square
        If so, set it to that value
        */
        public void onlyPossible()
        {
            if (myValue == Square.NULL_VALUE)//only run if the square has no value
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
        }
        /*
            Set the value of the square to integer value and set the possible array to all false except value
        */
        public void setValue(int value)
        {
            if (value >= 0 && value < n * n)//check legality of value
            {
                if (myValue == NULL_VALUE)
                {
                    myValue = value;
                    possibleValues = setAllInBoolArrayTo(possibleValues, false); //all the others are no longer valid
                    possibleValues[myValue] = true; //set the value to true
                    updateUnhandled = true;
                }
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
        /*
            A method to get a particular grouping value of a square by using constants described in the UnitType Class
        */
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
            return -1;//a legal group value was not passed
        }
        /*
            How many values in the possible value array are true?
        */
        public int numberOfPossibilities()
        {
            int count = 0;
            foreach (bool b in possibleValues)//iterate through each
            {
                if (b)
                {
                    count++;
                }
            }
            return count;
        }
        /*
            An array of bools can be thought of as a binary number. Each permutation of the array maps to a binary number
            with a decimal limit of 2^(n^2). This value can be used to quickly compare squares
        */
        public int binaryCompare()
        {
            int representation = 0;
            for (int i = 0; i < n2; i++)
            {
                if (possibleValues[i])
                {
                    representation += (int)Math.Pow(2, i);
                }
            }
            return representation;
        }
        public override string ToString()
        {
            return "(" + myRow + ", " + myColumn + ", " + myBlock + ")" + " is " + (myValue + 1);
        }
        /*
            Some classes use 1d arrays to store squares. Position provides for unique values of rows and columns
        */
        public int position()
        {
            return myColumn + n2 * myRow;
        }
        public bool getUpdateStatus()
        {
            return updateUnhandled;
        }
        /**
            Any update has been recognized and handled
        */
        public void updateHandled()
        {
            updateUnhandled = false;
        }
        //compare a squares position in an array
        //Useful for sorting
        public int CompareTo(object other)
        {
            Square otherSquare = other as Square;
            return this.position().CompareTo(otherSquare.position());
        }
    }
}
