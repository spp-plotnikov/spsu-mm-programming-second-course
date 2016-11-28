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
        private List<WorkingThread> threads;
        private Mutex mutex;
        private int numThreads = 5;

        public ThreadPool()
        {
            tasks = new Queue<Action>();
            threads = new List<WorkingThread>();
            mutex = new Mutex();
        }

        public void Start()
        {
            for (int i = 0; i < numThreads; i++)
            {
                WorkingThread thread = new WorkingThread(tasks, i, mutex);
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
            threads.Clear();
        }
    }
}
