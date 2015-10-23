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
            Board b = new Board(2);
            b.giveInitial(0, 0, 3);
            b.giveInitial(0, 1, 4);
            b.giveInitial(0, 2, 1);
            b.giveInitial(1, 1, 2);
            b.giveInitial(2, 2, 2);
            b.giveInitial(3, 1, 1);
            b.giveInitial(3, 2, 4);
            b.giveInitial(3, 3, 3);
            Console.WriteLine(b.ToString());
            while (repeat.Equals(""))
            {

                b.nextStep();
                Console.WriteLine(b.ToString());
                repeat = Console.ReadLine();
            }
        }
    }
}
