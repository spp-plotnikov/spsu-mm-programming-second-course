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
        private static Mutex taskMutex = new Mutex(false);
        ManualResetEvent notEmptyTaskQueue = new ManualResetEvent(false);

        public ThreadPool(int numOfThreads) // Constructor
        {
            this.numOfThreads = numOfThreads;
        }

        public void AddTask(ThreadStart threadStart) // Adding task with some function
        {
            taskMutex.WaitOne();
            if (disposed) { throw new ObjectDisposedException("ThreadPool"); }
            queueOfTasks.Enqueue(threadStart);
            taskMutex.ReleaseMutex();
            notEmptyTaskQueue.Set();
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
                taskMutex.WaitOne();
                if (queueOfTasks.Count != 0)
                {
                    ThreadStart start = queueOfTasks.Dequeue();
                    taskMutex.ReleaseMutex();
                    start();
                }
                else
                {
                    taskMutex.ReleaseMutex();
                    notEmptyTaskQueue.Reset();
                    notEmptyTaskQueue.WaitOne();
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
            notEmptyTaskQueue.Set();
        }
    }
}
