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

            Console.WriteLine("Press any key to give additional works");
            Console.WriteLine("Press any key once more to exit/abort");

            for(int workNum = 10; workNum < 31; workNum++)
            {
                string num = workNum.ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine(num + " start   {0}", Thread.CurrentThread.Name);
                    Thread.Sleep(rnd.Next(99, 1999));
                    Console.WriteLine(num + " finish  {0}", Thread.CurrentThread.Name);
                });
            }
            pool.Start();

            Console.ReadKey();

            for (int workNum = 100; workNum < 121; workNum++)
            {
                string num = workNum.ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine(num + " start   {0}", Thread.CurrentThread.Name);
                    Thread.Sleep(rnd.Next(99, 1999));
                    Console.WriteLine(num + " finish  {0}", Thread.CurrentThread.Name);
                });
            }
            Console.WriteLine("Extra work added.");

            Console.ReadKey();
            pool.Dispose();
        }
    }
}
