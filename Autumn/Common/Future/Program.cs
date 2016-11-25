using System;
using System.Diagnostics;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[200];
            for (int i = 0; i < 200; i++)
            {
                arr[i] = 1;
            }
            AsyncCalculating asyncCalculating = new AsyncCalculating();
            asyncCalculating.CalculateSum(arr);
            Console.WriteLine(asyncCalculating.GetSum);
            Console.ReadKey();
        }
    }
}
