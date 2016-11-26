using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesSum
{
    class Program
    {
        private static int numELem = 10000;
        static void Main(string[] args)
        {
            int[] arr = new int[numELem];
            for (int i = 0; i < numELem; i++)
                arr[i] = 1;

            FirstSum sumFirst = new FirstSum();
            int ansFirst = sumFirst.Sum(arr);
            Console.WriteLine("First sum result = " + ansFirst);

            SecondSum sumSecond = new SecondSum();
            int ansSecond = sumSecond.Sum(arr);
            Console.WriteLine("Second sum result = " + ansSecond);

            Console.ReadKey();
        }
    }
}
