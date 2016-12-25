using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[1000];
            for(int i = 0; i < 1000; i++)
            {
                array[i] = i + 1;
            }
            var sum = new ArraySum();
            var sumPrefix = new PrefixArraySum();
            Console.WriteLine(sum.Sum(array));
            Console.WriteLine(sumPrefix.Sum(array));
            Console.ReadLine();
        }
    }
}
