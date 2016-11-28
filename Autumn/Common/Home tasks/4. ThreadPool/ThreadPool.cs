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
        private int numThreads = 5;

        public ThreadPool()
        {
            tasks = new Queue<Action>();
            threads = new List<WorkingThread>();
        }

        public void Start()
        {
            for (int i = 0; i < numThreads; i++)
            {
                WorkingThread thread = new WorkingThread(tasks, i);
                threads.Add(thread);
                thread.Start();
            }
        }

        public void Enqueue(Action task)
        {
            lock(tasks)
            {
                tasks.Enqueue(task);
                Monitor.Pulse(tasks);
            }
        }

        public void Dispose()
        {
            lock (tasks)
            {
                tasks.Clear();
                for (int i = 0; i < numThreads; i++)
                {
                    threads[i].Stop();
                }
                Monitor.PulseAll(tasks);
            }
            threads.Clear();
        }
    }
}
