using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            // start a new instance of thread Pool
            ThreadPool threadPool = new ThreadPool();

            Console.WriteLine("To start the creation of tasks provide a key.");
            Console.WriteLine("Press any key to finish this process");
            Console.WriteLine();
            Console.ReadKey();

            while (Console.KeyAvailable == false)
            {
                threadPool.AddToPool(MyAction.DoTheJob);
                Thread.Sleep(500);
            }

            Console.WriteLine();
            threadPool.Dispose();
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
