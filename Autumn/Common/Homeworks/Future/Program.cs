using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace future
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfElem = 1000;
            int[] arr = new int[numOfElem];
            for (int i = 0; i < numOfElem; i++)
            {
                arr[i] = i;
            }
            RunTheProgramm main = new RunTheProgramm(arr);
            main.Run();
            Console.WriteLine("Program finished!");
            Console.ReadKey();
        }
    }
}
