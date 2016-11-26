using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArrSum
{
    class Program
    {
        static int[] CreateArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = 1;
            }
            return array;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Input size of Array");
            int s = Convert.ToInt32(Console.ReadLine());
            int[] arr = CreateArray(s);
            int res1 = new Summator1().Sum(arr);
            Console.WriteLine("sum1: " + res1);
            int res2 = new Summator2().Sum(arr);
            Console.WriteLine("sum2: " + res2);
            Console.ReadKey();

        }
    }
}
