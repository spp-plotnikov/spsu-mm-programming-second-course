using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUM
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 1028;
            int[] arr = new int[size];

            for (int i = 0; i < size; i++)
                arr[i] = (arr[i / 2] + 1);

            Console.WriteLine("Sum: " + arr.Sum().ToString());
            Console.WriteLine("LogSum: " + new SumLog().Sum(arr).ToString());
            Console.WriteLine("RecSum: " + new SumRec().Sum(arr).ToString());
        }
    }
}
