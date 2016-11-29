using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class MyThread
    {
        private Queue<Action> tasks = new Queue<Action>();
        private Thread curThread;
        private bool work = true;
        private int name;

        public MyThread(Queue<Action> tasks, int name)
        {
            this.name = name;
            this.tasks = tasks;
            Thread curThread = new Thread(Run);
            this.curThread = curThread;
            this.curThread.Start();
        }

        public void SetWork()
        {
            work = false;
        }

        public void Run()
        {
            while (work)
            {
                Monitor.Enter(tasks);
                if (tasks.Count() > 0)
                {
                    Action task = tasks.Dequeue();
                    Monitor.Pulse(tasks);
                    Monitor.Exit(tasks);

                    Console.WriteLine("{0} has new work", name);
                    task();
                    Thread.Sleep(500);
                }
                else
                {
                    Monitor.Exit(tasks);
                    lock (tasks)
                    {
                        Monitor.Wait(tasks);
                    }
                }
            }
        }

        public void Stop()
        {
            curThread.Join();
        }
    }
}
