using System;

namespace SudokuSolver
{
    public class Board
    {
        /**
            Class that represents the sudoku board.
            Stores the squares and delgates them to unit types in alternating order
            
        */
        public int[] typesAvailable = { UnitType.COLUMN, UnitType.ROW, UnitType.BLOCK };//the three organizational units of the board
        Square[] allSquares;//the array of the squares on the board
        private int n, n2 ,n4;//values of n (typical sudoku boards are n = 3). n2 is n * n (The number of organizational units of a type and the max value of a square). n4, n2 * n2, is the number of squares on a board.
        private int numberOfIterations = 0; //how many alternations between types have been made
        private bool complete;//is the board in a completely solved state?
        public Board(int n)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            allSquares = new Square[n4];
            fillSquares();//fill the squares with new squares
            complete = false;//has to be false in this case as the board is empty
        }
        /**
            Go to the next iteration if the board is not complete
        */
        public void nextStep()
        {
            complete = completionStatus();//figure out if the board is complete
            if (!complete)
            {
                UnitType current;
                current = new UnitType(allSquares, n, typesAvailable[numberOfIterations % typesAvailable.Length]);//take the next unit type and set current to that
                allSquares = current.operate();//set allSquares of the board equal to what it calculates
                numberOfIterations++;
                complete = completionStatus(); //update completion status
            }
            else//the board is complete. Don't iterate anymore
            {
                Console.WriteLine("Completed!");//
            }
        }
        public void completePuzzle()
        {//need a way to detect if the puzzle is looping without change
            Square[] tempSquares;
            int count = 0;
            while(!completionStatus() && count < 10)
            {
                tempSquares = allSquares;
                nextStep();
                if (allSquares.Equals(tempSquares))
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
            }
        }
        /**
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
        /**
            The completion status of the board is returned
        */
        public bool isComplete()
        {
            complete = completionStatus();
            return complete;
        }
        /**
            Populate the squares array with null values and relevant positions
        */
        private void fillSquares()
        {
            for (int position = 0; position < n4; position++)//iterate through each element in the array
            {
                allSquares[position] = new Square(position / n2, position % n2, n);//create a new square with proper row and column values
            }
        }
        /*
         A way to enter in initial value
         TODO get a better, graphical way to do this
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
        /**
            Return the squares array. Going to be useful for when graphics are implemented
        */
        public Square[] getSquares()
        {
            Array.Sort<Square>(allSquares);
            return allSquares;
        }
        /**
            A fancier ToString() method
            This actually looks a little like a sudoku board
        */
        public string showSquares()
        {
            int[,] arranged = new int[n2,n2];//a visual way to imagine a sudoku board is a two dimmensional array
            Square temp;//the current square being iterated to
            for (int i = 0; i < n4; i++)
            {
                temp = allSquares[i];
                arranged[temp.getRow(),temp.getColumn()] = temp.getValue() + 1;//get the temporary squares row and column and shove it into that array
            }
            string display = "";
            for (int row = 0; row < n2; row++)
            {
                for (int column = 0; column < n2; column++)
                {
                    display = display + " | " + arranged[row, column];
                }
                display = display + " |\n";
            }
            return display;
        }
        public void setupBoardFromString(string initialState)
        {
            for (int i = 0; i < initialState.Length; i++)
            {
                if (!initialState[i].Equals('0'))
                {
                    giveInitial(i / n2, i % n2, int.Parse(initialState[i].ToString()));
                }
            }
        }
    }
}
