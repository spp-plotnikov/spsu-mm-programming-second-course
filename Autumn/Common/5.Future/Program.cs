using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            // init pool
            ThreadPool pool = new ThreadPool();

            // example of task
            Action task = new Action(() => {
                Console.WriteLine("Doing something...");
                Thread.Sleep(500);
            });

            // push to pool
            for (int i = 0; i < 30; ++i)
                pool.Enqueue(task);

            // press key before disposing pool
            Console.ReadKey();
            pool.Dispose();
        }
    }
}
