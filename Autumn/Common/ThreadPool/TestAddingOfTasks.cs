using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    static class TestAddingOfTasks
    {
        public static void AddTasks(ThreadPool threadPool)
        {
            for (int i = 0; i < 10; i++) // Adding tasks
            {
                if (i % 2 == 0)
                {
                    threadPool.AddTask(new ThreadStart(Tasks.Something1));
                }
                else
                {
                    threadPool.AddTask(new ThreadStart(Tasks.Something2));
                }
            }
        }
    }
}
