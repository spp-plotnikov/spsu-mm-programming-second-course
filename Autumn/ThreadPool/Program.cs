using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool pool = new ThreadPool(10);
            Random rng = new Random();
            for (int i = 0; i < 20; i++)
            {
                string num = i.ToString();
                pool.Enqueue(() => {
                    Console.WriteLine("Work №" + num + " begin");
                    Thread.Sleep(rng.Next(200, 15000));
                    Console.WriteLine("Work №" + num + " finished");
                });
            }

            pool.Start();
            Console.ReadKey();
            pool.Dispose();

            

        }
    }
}
