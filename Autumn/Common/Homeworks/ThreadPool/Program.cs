using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            // start a new instance of thread Pool
            ThreadPool threadPool = new ThreadPool();
            int numOfTasks = 6;

            // do all the tasks
            for (int i = 0; i < numOfTasks; i++)
            {
                threadPool.AddToPool(MyAction.DoTheJob);
            }

            // wait for tasks to be completed
            Thread.Sleep(2800);
            threadPool.Dispose();
            Console.ReadLine();
        }
    }
}
