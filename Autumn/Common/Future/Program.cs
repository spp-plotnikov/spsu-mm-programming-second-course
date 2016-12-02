using System;
using System.Threading;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2500);
            int[] arr = new int[200];
            for (int i = 0; i < 200; i++)
            {
                arr[i] = 1;
            }
            AsyncCalculating asyncCalculating = new AsyncCalculating(ArraySumImplementation.FirstSum);
            asyncCalculating.CalculateSum(arr);
            Console.WriteLine(asyncCalculating.Sum);
            Console.ReadKey();
        }
    }
}
