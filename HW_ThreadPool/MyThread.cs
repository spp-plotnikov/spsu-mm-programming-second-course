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

        public void Run()
        {
            while (work)
            {
                Monitor.Enter(tasks);
                if (tasks.Count > 0)
                {
                    Console.WriteLine("Thread {0} has new task", this.name);
                    Action curAction = tasks.Dequeue();
                    Monitor.Exit(tasks);
                    curAction();
                }
                else
                {
                    Monitor.Exit(tasks);
                    Thread.Sleep(500);
                }
            }
        }

        public void Stop()
        {
            work = false;
            curThread.Join();
        }
    }
}
