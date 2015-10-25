using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string repeat = "";
            Board b = new Board(3);
            //HIDDEN PRIME TEST
            //b.giveInitial(0, 3, 5);
            //b.giveInitial(0, 5, 3);
            //b.giveInitial(0, 7, 9);

            //b.giveInitial(1, 4, 6);
            //b.giveInitial(1, 5, 7);
            //b.giveInitial(1, 6, 1);
            //b.giveInitial(1, 7, 5);

            //b.giveInitial(2, 1, 5);
            //b.giveInitial(2, 2, 4);
            //b.giveInitial(2, 3, 9);
            //b.giveInitial(2, 4, 2);
            //b.giveInitial(2, 5, 1);
            //b.giveInitial(2, 7, 3);

            //b.giveInitial(3, 0, 8);
            //b.giveInitial(3, 1, 4);
            //b.giveInitial(3, 2, 9);
            //b.giveInitial(3, 3, 3);
            //b.giveInitial(3, 4, 7);
            //b.giveInitial(3, 6, 2);

            //b.giveInitial(4, 0, 1);
            //b.giveInitial(4, 1, 3);
            //b.giveInitial(4, 7, 7);
            //b.giveInitial(4, 8, 9);

            //b.giveInitial(5, 1, 7);
            //b.giveInitial(5, 2, 5);
            //b.giveInitial(5, 4, 1);
            //b.giveInitial(5, 5, 9);
            //b.giveInitial(5, 6, 4);
            //b.giveInitial(5, 8, 3);

            //b.giveInitial(6, 3, 6);
            //b.giveInitial(6, 4, 5);
            //b.giveInitial(6, 5, 4);
            //b.giveInitial(6, 6, 3);
            //b.giveInitial(6, 7, 2);

            //b.giveInitial(7, 1, 6);
            //b.giveInitial(7, 3, 7);
            //b.giveInitial(7, 4, 3);
            //b.giveInitial(7, 5, 2);
            //b.giveInitial(7, 6, 9);

            //b.giveInitial(8, 1, 2);
            //b.giveInitial(8, 3, 1);
            //b.giveInitial(8, 4, 9);
            //b.giveInitial(8, 5, 8);

            b.giveInitial(0, 0, 9);
            b.giveInitial(0, 1, 8);
            b.giveInitial(0, 3, 4);
            b.giveInitial(0, 4, 7);
            b.giveInitial(0, 5, 2);

            b.giveInitial(1, 2, 7);
            b.giveInitial(1, 6, 2);
            b.giveInitial(1, 8, 8);

            b.giveInitial(2, 0, 2);
            b.giveInitial(2, 1, 6);
            b.giveInitial(2, 4, 8);
            b.giveInitial(2, 7, 9);
            b.giveInitial(2, 8, 7);

            b.giveInitial(3, 0, 4);
            b.giveInitial(3, 4, 3);
            b.giveInitial(3, 7, 7);
            b.giveInitial(3, 8, 5);

            b.giveInitial(4, 2, 6);
            b.giveInitial(4, 3, 7);
            b.giveInitial(4, 5, 4);
            b.giveInitial(4, 6, 8);
            b.giveInitial(4, 7, 2);

            b.giveInitial(5, 0, 7);
            b.giveInitial(5, 4, 2);
            b.giveInitial(5, 8, 1);

            b.giveInitial(6, 0, 8);
            b.giveInitial(6, 1, 7);
            b.giveInitial(6, 4, 4);
            b.giveInitial(6, 7, 3);

            b.giveInitial(7, 0, 5);
            b.giveInitial(7, 2, 9);
            b.giveInitial(7, 6, 7);

            b.giveInitial(8, 0, 6);
            b.giveInitial(8, 3, 9);
            b.giveInitial(8, 5, 7);
            b.giveInitial(8, 7, 8);


            //int value;
            //for (int row = 1; row <= 9; row++)
            //{
            //    for (int column = 1; column <= 9; column++)
            //    {
            //        Console.Write("(" + row + ", " + column + ") = ");
            //        value = int.Parse(Console.ReadLine());
            //        if (value > 0)
            //        {
            //            b.giveInitial(row - 1, column - 1, value);
            //        }
            //    }
            //}

            //VERY HARD PUZZLE
            //b.giveInitial(0, 0, 8);

            //b.giveInitial(1, 2, 3);
            //b.giveInitial(1, 3, 6);

            //b.giveInitial(2, 1, 7);
            //b.giveInitial(2, 4, 9);
            //b.giveInitial(2, 6, 2);

            //b.giveInitial(3, 1, 5);
            //b.giveInitial(3, 5, 7);

            //b.giveInitial(4, 4, 4);
            //b.giveInitial(4, 5, 5);
            //b.giveInitial(4, 6, 7);

            //b.giveInitial(5, 3, 1);
            //b.giveInitial(5, 7, 3);

            //b.giveInitial(6, 2, 1);
            //b.giveInitial(6, 7, 6);
            //b.giveInitial(6, 8, 8);

            //b.giveInitial(7, 2, 8);
            //b.giveInitial(7, 3, 5);
            //b.giveInitial(7, 7, 1);

            //b.giveInitial(8, 1, 9);
            //b.giveInitial(8, 6, 4);

            ////CHEAT VALUES
            ////b.giveInitial(8, 8, 2);
            //b.giveInitial(0, 7, 4);
            //b.giveInitial(0, 2, 2);
            //b.giveInitial(4, 2, 9);
            //b.giveInitial(5, 0, 2);

            //Board b = new Board(2);

            //b.giveInitial(0, 1, 2);
            //b.giveInitial(0, 2, 4);
            //b.giveInitial(1, 0, 1);
            //b.giveInitial(1, 3, 3);
            //b.giveInitial(2, 0, 4);
            //b.giveInitial(2, 3, 2);
            //b.giveInitial(3, 1, 1);
            //b.giveInitial(3, 2, 3);

            Console.WriteLine(b.showSquares());
            while (!b.isComplete())
            {
                b.nextStep();
                Console.WriteLine(b.showSquares());
                Console.ReadLine();
            }
            //Square[] sArr = (b.getSquares());
            //string s = sArr[68].position().ToString() + ". ";
            //foreach (bool bo in sArr[68].getPossibleValues())
            //{

            //    s = s + " " + bo.ToString();
            //}
            //Console.WriteLine(s);
            Console.WriteLine(b.showSquares());
            Console.WriteLine("!!!!!!!!!!!\n!!!!!!!!");
            repeat = Console.ReadLine();
        }
    }
}
