using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool pool = new ThreadPool();
            Random rng = new Random();
            for (int i = 0; i < 20; i++)
            {
                string num = i.ToString();
                pool.Enqueue(() => {
                    Console.WriteLine("Work N" + num + " start");
                    Thread.Sleep(rng.Next(200,1500));
                    Console.WriteLine("Work N" + num + " finish");
                });
            }
            Console.WriteLine("Push any key to finish");
            pool.Start();
            Console.ReadKey();
            pool.Dispose();
        }
    }
}
