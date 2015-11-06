using System;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
            private static int n = 3;
        private static int n2 = n * n;
        static void Main(string[] args)
        {
            Board b = new Board(n);
            StreamReader inputPuzzles;
            Square[] firstThree = new Square[3];
            int sum = 0;
            inputPuzzles = new StreamReader("p096_sudoku.txt");
            String temp, puzzleString;
            for (int currentPuzzle = 0; !inputPuzzles.EndOfStream; currentPuzzle++)
            {
                puzzleString = "";
                temp = "";
                temp = inputPuzzles.ReadLine();
                while (!inputPuzzles.EndOfStream && !temp.StartsWith("G"))
                {
                    puzzleString = puzzleString + temp;
                    temp = inputPuzzles.ReadLine();
                }
                if (puzzleString.Length > n)
                {
                    b = new Board(3);
                    b.setupBoardFromString(puzzleString);

                    //b.showSquares();
                    b.completePuzzle();
                    Console.WriteLine(b.showSquares());
                    temp = "";
                    //Console.WriteLine(b.showSquares());
                    for (int index = 0; index < 3; index++)
                    {
                        if ((b.getSquares()[index].getValue() + 1) == 0)
                        {
                            Console.WriteLine(b.showSquares());
                            break;
                        }
                        temp = temp + (b.getSquares()[index].getValue() + 1);

                    }
                    //Console.WriteLine(temp);
                    sum += int.Parse(temp);
                    //Console.WriteLine(sum);
                    // Console.ReadLine();
                }

            }
            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
