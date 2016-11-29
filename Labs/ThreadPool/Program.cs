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
            Console.WriteLine("Press key to stop");                

            var poolManager = new PoolManager(numPool);
            poolManager.StartWork();
            Console.ReadKey();
            poolManager.StopWork();
            Console.WriteLine("finita");
            Console.ReadKey();


        }
    }
}
