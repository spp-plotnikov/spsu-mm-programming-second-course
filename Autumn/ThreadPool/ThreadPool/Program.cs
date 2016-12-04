using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var threadPool = new ThreadPool())
            {
                Action action = () =>
                {
                    Thread.Sleep(400);
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " thread finished the task!");
                };

                threadPool.Start();

                var isFinished = (object)(false);

                Thread thr = new Thread(() =>
                {
                    while (!(bool)isFinished)
                    {
                        Thread.Sleep(200);
                        threadPool.Enqueue(action);
                    }
                });

                thr.Start();
  
                Console.ReadLine();
                isFinished = true;
            }
        }
    }
}
