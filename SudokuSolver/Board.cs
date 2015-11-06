using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    public class Board
    {
        /**
            Class that represents the sudoku board.
            Stores the squares and delgates them to unit types in alternating order
        */
        public const int GUESS_MARK = 10;//the number of moves without a change it takes to start guessing
        private int count;//stores the number of moves since a change
        public int[] typesAvailable = { UnitType.COLUMN, UnitType.ROW, UnitType.BLOCK };//the three organizational units of the board
        Square[] allSquares;//the array of the squares on the board
        private int n, n2 ,n4;//values of n (typical sudoku boards are n = 3). n2 is n * n (The number of organizational units of a type and the max value of a square). n4, n2 * n2, is the number of squares on a board.
        private int numberOfIterations = 0; //how many alternations between types have been made
        private bool complete;//is the board in a completely solved state?
        //Constructors
        public Board(int n, Square[] squares)
        {
            initalize(n, squares);
        }
        public Board(int n)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            complete = false;//has to be false in this case as the board is empty
            allSquares = new Square[n4];
            fillSquares();//fill the squares with new squares
            
        }
        private void initalize(int n, Square[] squares)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            allSquares = squares;
            complete = false;//has to be false in this case as the board is empty
        }
        /**
            Go to the next iteration if the board is not complete
        */
        public bool nextStep()
        {
            Square[] comparatorSquares = new Square[allSquares.Length];
            Array.Copy(allSquares, comparatorSquares, allSquares.Length);
            complete = completionStatus();//figure out if the board is complete
            if (!complete && isLegal() && count < GUESS_MARK + 1)
            {
                UnitType current;
                current = new UnitType(allSquares, n, typesAvailable[numberOfIterations % typesAvailable.Length]);//take the next unit type and set current to that
                allSquares = current.operate();//set allSquares of the board equal to what it calculates
                numberOfIterations++;
                complete = completionStatus(); //update completion status
                if (!current.HaveChangesBeenMade())
                {
                    count++;
                    Console.WriteLine(count);
                }
                if (count >= GUESS_MARK)
                {
                    //Guess();
                    //count = 0;
                }
                return false;//not complete
            }
            else//the board is complete. Don't iterate anymore
            {
                return true;//complete
            }

        }
        public void completePuzzle()
        {//need a way to detect if the puzzle is looping without change
            while (!nextStep()) ;
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
   
        private void Guess()
        {
            //find a square with two possible values
            Console.WriteLine("Guessing!");
            Square[] temp;
            Board branch;
            List<Square[]> permutations = new List<Square[]>();
            for (int i = 0; i < n4; i++)
            {
                if (allSquares[i].getValue() != Square.NULL_VALUE)
                {

                    foreach (int values in allSquares[i].GetPossibleValuesList())
                    {
                        temp = new Square[n4];
                        Array.Copy(allSquares, temp, n4);
                        temp[i].setValue(i);
                        permutations.Add(temp);
                    }
                }
            }
            foreach (Square[] sArr in permutations)
            {
                branch = new Board(n, sArr);
                branch.completePuzzle();
                if (branch.isLegal() && branch.isComplete())
                {
                    allSquares = branch.allSquares;
                    Console.WriteLine(showSquares() + "\n\n!!!!!!!");
                    Console.ReadLine();
                }

            }
        }
        public bool isLegal()
        {
            int[][] values = new int[n2 * 3][];//there are n2 * 3 units on the board
            for (int arrIndex = 0; arrIndex < values.Length; arrIndex++)
            {
                values[arrIndex] = new int[n2];
            }
            int value;
            Square s;
                for (int squareIndex = 0; squareIndex < n4; squareIndex++)
                {
                    s = allSquares[squareIndex];
                    value = s.getValue();
                    if (value != Square.NULL_VALUE)
                    {
                        values[s.getColumn() + UnitType.COLUMN * n2][value]++;
                        values[s.getRow() + UnitType.ROW * n2][value]++;
                        values[s.getBlock() + UnitType.BLOCK * n2][value]++;
                    }
                }
            foreach (int[] arr in values)
            {
                foreach (int count in arr)
                {
                    if (count > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
            //go through rows
        }
    }
}
