using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input number of threads in Pool");
            int numPool = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input number of tasks");
            int numTask = Convert.ToInt32(Console.ReadLine());

            new PoolManager(numPool, numTask).StartWork();
            Console.WriteLine("finita");
            Console.ReadKey();

        }
    }
}
