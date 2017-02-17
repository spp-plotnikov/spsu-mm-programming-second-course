using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool pool = new ThreadPool(10);
            for (int i = 0; i < 10; i++)
            {
                pool.Enqueue(() => Thread.Sleep(1000));
            }
            Thread.Sleep(5000);

            for (int i = 0; i < 10; i++)
            {
                pool.Enqueue(() => Thread.Sleep(1000));
            }
            Console.ReadKey();
        }
    }
}
