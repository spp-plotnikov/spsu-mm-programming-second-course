using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    class Tasks
    {
        private ThreadPool pool;
        private Thread task;
        private bool isWorking;
        private readonly int delay = 500;

        public Tasks(ThreadPool newPool)
        {
            isWorking = true;
            pool = newPool;
            task = new Thread(() => Run());
        }

        public void Start()
        {
            task.Start();
        }

        public void Stop()
        {
            isWorking = false;
        }

        public void ThreadJoin()
        {
            task.Join();
        }

        private Action newAct()
        {
            Action tmp = () =>
            {
                Thread.Sleep(delay);
                Console.WriteLine("Done");
            };
            return tmp;
        }

        private void Run()
        {
            while (isWorking)
            {
                Console.WriteLine("New task");
                pool.Enqueue(newAct());
                Thread.Sleep(delay);
            }
        }
    }
}
