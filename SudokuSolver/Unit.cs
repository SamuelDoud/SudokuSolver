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
        bool isComplete;
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
        public Square[] elimination()
        {
            setImpossibles();//need a way to post changes
            updateValueTable();
            bool updateMade = false;
            int counter = 0, position, value;
            int lastIndex;
            for (value = 0; value < n2; value++)
            {//check for only one of a number in the array
                lastIndex = 0;
                for (position = 0; position < n2; position++)
                {
                    if (valueTable[position][value] && myMembers[position].getValue() != value)
                    {
                        counter++;//this value now has one more possible position on the unit
                        lastIndex = position;
                    }
                }
                if (counter == 1)
                {
                    for (int i = 0; i < n2; i++)
                    {//all the other members cannot have this value anymore. Make it impossible to have it.
                        myMembers[i].impossibleValue(value);
                    }
                    myMembers[lastIndex].setValue(value);
                    //clearly the position  that we determined has this value
                    updateMade = true;//set updatemade to true. Some other squares may have a value now.
                    value = n2 + 1;//this will cause the loop to exit and will repeat the function
                }
            }
            if (updateMade)
            {
                myMembers = elimination();
            }
            return myMembers;
        }
        public void setImpossibles()
        {
            int value = Square.NULL_VALUE;
            for (int primary = 0; primary < n2; primary++)
            {
                value = myMembers[primary].getValue();
                if (value != Square.NULL_VALUE)
                {
                    for (int secondary = 0; secondary < n2; secondary++)
                    {
                        if (secondary != primary)
                        {
                            myMembers[secondary].impossibleValue(value);
                        }
                    }
                }
            }
        }

    }
}
