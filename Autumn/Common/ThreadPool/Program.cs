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
            TestAddingOfTasks.AddTasks(threadPool);
            threadPool.StartWorking();
            Thread.Sleep(500);
            threadPool.Dispose();
            Console.ReadKey();
        }
    }
}
