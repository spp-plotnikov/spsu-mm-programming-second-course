using System;
using System.Threading;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isFinished = false;
            int numberOfThreads = new Random().Next(5, 50);
            Console.WriteLine("Number of threads: {0}", numberOfThreads);
            Console.WriteLine("Press any button to start/finish");
            Console.ReadKey();

            ThreadPool threadPool = new ThreadPool(numberOfThreads);
            threadPool.Start();

            Action action = delegate ()
            {
                Console.WriteLine("Thread finished action");

                Thread.Sleep(100);
            };

            Thread thread = new Thread(delegate ()
            {
                while (!isFinished)
                {
                    Thread.Sleep(100);
                    threadPool.Enqueue(action);
                }
            });
            thread.Start();

            Console.ReadKey();
            isFinished = true;
            threadPool.Dispose();
            Console.WriteLine("The end");
            Console.ReadLine();
        }
    }
}
