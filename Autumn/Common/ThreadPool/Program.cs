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
            TestAddingOfTasks.AddTasks(threadPool); // Adding tasks
            threadPool.StartWorking(); // Starting
            Console.ReadKey(); 
            TestAddingOfTasks.AddTasks(threadPool); // Adding tasks during work
            Console.ReadKey();
            threadPool.Dispose(); // Disposing
            Console.ReadKey();
        }
    }
}
