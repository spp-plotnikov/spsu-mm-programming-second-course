using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace ThreadPool
{
    class ThreadPool : IDisposable
    {
        private Queue<ThreadStart> queueOfTasks = new Queue<ThreadStart>();
        private Queue<Thread> queueOfFreeThreads = new Queue<Thread>();
        private List<Thread> listOfThreads = new List<Thread>();
        private bool disposed = false;

        public ThreadPool(int numOfThreads) // Constructor
        {
            listOfThreads = Enumerable.Repeat(new Thread(new ThreadStart(delegate { })), numOfThreads).ToList();
        }

        private void initQueueOfFreeThreads() // When the queue is empty, reinitializate it
        {
            if (disposed) { throw new NotImplementedException(); }
            Console.WriteLine("Reinit freeThreads");
            foreach (Thread thread in listOfThreads)
            {
                if (!thread.IsAlive) { queueOfFreeThreads.Enqueue(thread); }
            }
        }
   
        public void AddTask(ThreadStart threadStart) // Adding task with some function
        {
            if (disposed) { throw new NotImplementedException(); }
            queueOfTasks.Enqueue(threadStart);
        }

        public void StartWorking() // Start
        {
            initQueueOfFreeThreads();
            while (!disposed)
            {
                //Console.WriteLine(isWorking);
                if (queueOfTasks.Count != 0)
                {
                    try
                    {
                        Thread thread = queueOfFreeThreads.Dequeue();
                        ThreadStart curTask = queueOfTasks.Dequeue();
                        thread = new Thread(curTask);
                        thread.Start();
                    }
                    catch
                    {
                        initQueueOfFreeThreads();
                    }
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
        }


    }
}
