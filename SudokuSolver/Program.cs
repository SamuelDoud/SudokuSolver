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
            b.giveInitial(0, 0, 5);
            b.giveInitial(0, 1, 3);
            b.giveInitial(0, 4, 7);

            b.giveInitial(1, 0, 6);
            b.giveInitial(1, 3, 1);
            b.giveInitial(1, 4, 9);
            b.giveInitial(1, 5, 5);

            b.giveInitial(2, 1, 9);
            b.giveInitial(2, 2, 8);
            b.giveInitial(2, 7, 6);

            b.giveInitial(3, 0, 8);
            b.giveInitial(3, 4, 6);
            b.giveInitial(3, 8, 3);

            b.giveInitial(4, 0, 4);
            b.giveInitial(4, 3, 8);
            b.giveInitial(4, 5, 3);
            b.giveInitial(4, 8, 1);

            b.giveInitial(5, 0, 7);
            b.giveInitial(5, 4, 2);
            b.giveInitial(5, 8, 6);

            b.giveInitial(6, 1, 6);
            b.giveInitial(6, 6, 2);
            b.giveInitial(6, 7, 8);

            b.giveInitial(7, 3, 4);
            b.giveInitial(7, 4, 1);
            b.giveInitial(7, 5, 9);
            b.giveInitial(7, 8, 5);

            b.giveInitial(8, 4, 8);
            b.giveInitial(8, 7, 7);
            b.giveInitial(8, 8, 9);

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
            }
            //Square[] sArr = (b.getSquares());
            //string s = sArr[68].position().ToString() + ". ";
            //foreach (bool bo in sArr[68].getPossibleValues())
            //{

            //    s = s + " " + bo.ToString();
            //}
            //Console.WriteLine(s);
            Console.WriteLine(b.showSquares());
            repeat = Console.ReadLine();
        }
    }
}
