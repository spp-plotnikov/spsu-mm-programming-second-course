using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            int lenght = 1000000;
            int[] arr = new int[lenght];
            for (int j = 0; j < lenght; j++)
                arr[j] = 1;
            Console.WriteLine("SumModule = {0}", new SumModule().Sum(arr));
            DateTime start = DateTime.Now;
            Console.WriteLine("Start time!");
            Console.WriteLine("SumKaskad = {0}", new SumKaskad().Sum(arr));
            Console.WriteLine(DateTime.Now - start);
            Console.ReadLine();
        }
    }
}
