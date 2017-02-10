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
            ThreadPool pool = new ThreadPool();
            for (int i = 0; i <= 5; i++)
            {
                string numAction = (i + 1).ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine("Action" + numAction + ": Work emulation");
                    Thread.Sleep(500);
                    Console.WriteLine("Action" + numAction + ": finish");
                });
            }
            Console.WriteLine("Pool start");
            pool.Start();
            Console.WriteLine("If you want finish press something.");
            Console.Read();
            pool.Dispose();
        }
    }
}
