using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    class WorkingThread
    {
        private Action task;
        private int name;
        private Thread thread;
        private bool isWorking;
        private Queue<Action> tasks;
        private readonly int delay = 500;
        private Mutex mutex;

        public WorkingThread(Queue <Action> que, int num, Mutex mtx)
        {
            name = num;
            thread = new Thread(() => Run());
            isWorking = true;
            tasks = que;
            mutex = mtx;
        }

        public void Start()
        {
            thread.Start();
        }

        private void Run()
        {
            while (isWorking)
            {
                if (tasks.Count() == 0)
                {
                    continue;
                }
                mutex.WaitOne();
                if (tasks.Count() == 0 || !isWorking)
                {
                    mutex.ReleaseMutex();
                    continue;
                }
                task = tasks.Dequeue();
                Console.WriteLine("Thread " + name + " is working on task");
                mutex.ReleaseMutex();
                task();
            }
        }

        public void Stop()
        {
            isWorking = false;
            thread.Join();
        }
    }
}
