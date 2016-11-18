using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace ThreadPool
{
    class ThreadPool : IDisposable
    {
        private Queue<ThreadStart> queueOfTasks = new Queue<ThreadStart>();
        private int numOfThreads;
        private bool disposed = false;
        private static Mutex addTaskMutex = new Mutex(false);
        private static Mutex getTaskMutex = new Mutex(false);
        public ThreadPool(int numOfThreads) // Constructor
        {
            this.numOfThreads = numOfThreads;
        }

        public void AddTask(ThreadStart threadStart) // Adding task with some function
        {
            addTaskMutex.WaitOne();
            if (disposed) { throw new NotImplementedException(); }
            queueOfTasks.Enqueue(threadStart);
            addTaskMutex.ReleaseMutex();
        }

        public void StartWorking() // Start
        {
            for (int i = 0; i < numOfThreads; i++)
            {
                Thread thread = new Thread(WorlkingCycle);
                thread.Start();
            }
        }

        private void WorlkingCycle()
        {
            while (!disposed)
            {
                getTaskMutex.WaitOne();
                try
                {
                    ThreadStart start = queueOfTasks.Dequeue();
                    start();
                }
                catch { }
                getTaskMutex.ReleaseMutex();
            }
        }

        public void Dispose()
        {
            disposed = true;
        }


    }
}
