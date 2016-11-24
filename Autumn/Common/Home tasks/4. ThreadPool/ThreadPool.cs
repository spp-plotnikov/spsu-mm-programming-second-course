using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    class ThreadPool : IDisposable
    {
        private Queue<Action> tasks;
        private List<Threads> threads;
        private Mutex mutex;
        private int numThreads = 5;

        public ThreadPool()
        {
            tasks = new Queue<Action>();
            threads = new List<Threads>();
            mutex = new Mutex();
        }

        public void Start()
        {
            for (int i = 0; i < numThreads; i++)
            {
                Threads thread = new Threads(tasks, i, mutex);
                threads.Add(thread);
                thread.Start();
            }
        }

        public void Enqueue(Action task)
        {
            mutex.WaitOne();
            tasks.Enqueue(task);
            mutex.ReleaseMutex();
        }

        public void Dispose()
        {
            mutex.WaitOne();
            tasks.Clear();
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Stop();
            }
            mutex.ReleaseMutex();
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].ThreadJoin();
            }
            threads.Clear();
        }
    }
}
