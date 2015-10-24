using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    //While units are rows, columns, or blocks, unit types are the n*n collection of these units. It delegates squares to a unit and returns them.
    
    class UnitType
    {
        public bool isComplete;
        public const int ROW = 0;
        public const int COLUMN = ROW + 1;
        public const int BLOCK = COLUMN + 1;
        private Square[] allSquares;
        private Square[][] groupedSquares;
        private int n, n2,n4, myType;
        public UnitType(Square[] squares, int n, int type)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            allSquares = squares;
            myType = type;
            groupedSquares = new Square[n2][];
            isComplete = checkCompletion();
            groupSquares();
        }
        public bool checkCompletion()
        {
            bool tempCompletion = true;
            foreach (Square s in allSquares)
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
        private void groupSquares()
        {
            int[] currentPositionInGroupArray = new int[n2];
            int tempGrouping;
            for (int i = 0; i < n2; i++)
            {
                groupedSquares[i] = new Square[n2];
            }
            for (int i = 0; i < n4; i++)
            {
                tempGrouping = allSquares[i].getGroupingValue(myType);
                
                groupedSquares[tempGrouping][currentPositionInGroupArray[tempGrouping]] = allSquares[i];
                currentPositionInGroupArray[tempGrouping]++;
            }
        }
        public Square[] operate()
        {
            //create a unit for each group value, thread this
            Unit[] unitsOfAType = new Unit[n2];
            int index = 0;
            foreach (Square[] sArr in groupedSquares)
            {
                unitsOfAType[index] = new Unit(sArr);
                index++;
            }
            for (int i = 0; i < n2; i++)
            {
                groupedSquares[i] = unitsOfAType[i].operate();
            }
            //wait for all eliminations to complete

            //stitch the all squares back together and return that
            allSquares = stitchJaggedArray(groupedSquares);
            //check completion status
            checkCompletion();//I need to make sure this is properly ordered
            return allSquares;
        }
        private Square[] stitchJaggedArray(Square[][] sJaggedArray)
        {
            List<Square> sList = new List<Square>();
            foreach (Square[] sArr in sJaggedArray)
            {
                sList.AddRange(sArr);
            }
            sList.Sort();
            return sList.ToArray();
        }

    }
}
