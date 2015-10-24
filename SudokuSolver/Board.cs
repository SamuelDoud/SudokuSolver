using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board
    {
        public int[] typesAvailable = { UnitType.ROW, UnitType.COLUMN, UnitType.BLOCK };
        Square[] allSquares;
        private int n, n2 ,n4;
        private int numberOfIterations = 0;
        private bool complete;
        public Board(int n)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            allSquares = new Square[n4];
            fillSquares();
            complete = false;//has to be false in this case as the board is empty
        }
        public void nextStep()
        {
            complete = completionStatus();
            if (!complete)
            {
                
                UnitType current;
                current = new UnitType(allSquares, n, typesAvailable[numberOfIterations % typesAvailable.Length]);//take the next unit type and set current to that
                allSquares = current.operate();//set allSquares of the board equal to what it calculates
                numberOfIterations++;
                complete = completionStatus(); //update completion status
            }
            else
            {
                Console.WriteLine("Completed you dummy");
            }
        }
        /*
        Set all the squares in the array to unique positions
        */
        private bool completionStatus()
        {
            foreach (Square s in allSquares)
            {
                if (s.getValue() == Square.NULL_VALUE)
                {
                    return false;
                }
            }
            return true;
        }
        public bool isComplete()
        {
            complete = completionStatus();
            return complete;
        }
        private void fillSquares()
        {
            for (int position = 0; position < n4; position++)
            {
                allSquares[position] = new Square(position / n2, position % n2, n);
            }
        }
        /*
        A way to enter in initial values
        */
        public void giveInitial(int row, int column, int value)
        {
            int position = row * n2 + column;
            allSquares[position].setValue(value - 1);
        }
        public override string ToString()
        {
            string allSquaresText = "";
            foreach (Square s in allSquares)
            {
                allSquaresText = allSquaresText + "\n" +  s.ToString();
            }
            return allSquaresText;
        }
        public Square[] getSquares()
        {
            return allSquares;
        }
        public string showSquares()
        {
            int[,] arranged = new int[n2,n2];
            Square temp;
            for (int i = 0; i < n4; i++)
            {
                temp = allSquares[i];
                arranged[temp.getRow(),temp.getColumn()] = temp.getValue() + 1;
            }
            string display = "";
            for (int row = 0; row < n2; row++)
            {
                for (int column = 0; column < n2; column++)
                {
                    display = display + " | " +arranged[row, column];
                }
                display = display + " |\n";
            }
            return display;
        }
    }
}
