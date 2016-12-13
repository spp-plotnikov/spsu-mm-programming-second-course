using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_5_Future
{
    class Program
    {
        static void Main(string[] args)
        {
            int arrsize = 10000;
            int[] arr = new int[arrsize];
            for(int i = 0; i < arrsize; i++)
            {
                arr[i] = i;
            }

            var sum1 = new ArrSum1();
            var sum2 = new ArrSum2();

            Console.WriteLine("Origin sum:{0}", arr.Sum());
            Console.WriteLine("My sum1:   {0}", sum1.Sum(arr));
            Console.WriteLine("My sum2:   {0}", sum2.Sum(arr));
            Console.ReadKey();
        }
    }
}
