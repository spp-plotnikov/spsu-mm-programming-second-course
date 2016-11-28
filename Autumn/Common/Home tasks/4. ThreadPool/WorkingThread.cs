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

        public WorkingThread(Queue <Action> que, int num)
        {
            name = num;
            thread = new Thread(() => Run());
            isWorking = true;
            tasks = que;
        }

        public void Start()
        {
            thread.Start();
        }

        private void Run()
        {
            while (isWorking)
            {
                lock (tasks)
                {
                    if (tasks.Count() == 0)
                    {
                        Monitor.Wait(tasks);
                    }
                    if (!isWorking)
                    {
                        return;
                    }
                    task = tasks.Dequeue();
                    Console.WriteLine("Thread " + name + " is working on task");
                }
                task();
            }
        }

        public void Stop()
        {
            isWorking = false;
        }
    }
}
