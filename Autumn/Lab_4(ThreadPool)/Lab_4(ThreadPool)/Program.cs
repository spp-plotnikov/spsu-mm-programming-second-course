using System;
using System.Threading;

namespace Lab_4_ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool pool = new ThreadPool();
            Random rnd = new Random();
            for(int workNum = 1; workNum < 25; workNum++)
            {
                string num = workNum.ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine(num + " start");
                    Thread.Sleep(rnd.Next(99, 1999));
                    Console.WriteLine(num + " finish");
                });
            }
            Console.WriteLine("Press any key to give additional works");
            Console.WriteLine("Press any key once more to exit/abort");
            pool.Start();
            Console.ReadKey();

            for(int workNum = 100; workNum < 125; workNum++)
            {
                string num = workNum.ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine(num + " start");
                    Thread.Sleep(rnd.Next(99, 1999));
                    Console.WriteLine(num + " finish");
                });
            }
            Console.WriteLine("Extra work added.");

            Console.ReadKey();
            pool.Dispose();
        }
    }
}
