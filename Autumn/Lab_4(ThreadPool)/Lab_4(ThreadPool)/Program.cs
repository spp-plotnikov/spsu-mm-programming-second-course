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
            for (int workNum = 1; workNum < 25; workNum++)
            {
                string num = workNum.ToString();
                pool.Enqueue(() =>
                {
                    Console.WriteLine(num + " start");
                    Thread.Sleep(rnd.Next(99, 1999));
                    Console.WriteLine(num + " finish");
                });
            }
            Console.WriteLine("Press any key to abort/exit");
            pool.Start();
            Console.ReadKey();
            pool.Dispose();
        }
    }
}
