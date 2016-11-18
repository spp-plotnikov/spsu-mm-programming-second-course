using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Tasks tasks = new Tasks();
            ThreadPool threadPool = new ThreadPool(5);

            for (int i = 0; i < 15; i++) // Adding tasks
            {
                if (i % 2 == 0)
                {
                    threadPool.AddTask(new ThreadStart(tasks.Something1));
                }
                else
                {
                    threadPool.AddTask(new ThreadStart(tasks.Something2));
                }
            }

            Thread poolThread = new Thread(new ThreadStart(threadPool.StartWorking));
            poolThread.Start();
            Thread.Sleep(100);
            threadPool.Dispose();
        }
    }
}
