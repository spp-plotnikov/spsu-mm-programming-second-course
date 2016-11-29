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
            Console.WriteLine(name);
        }
        public void Run()
        {
            while (work)
            {
                Action curAction = new Action(() => { });
                lock (tasks)
                {
                    if (tasks.Count > 0)
                    {
                        Console.WriteLine("Thread {0} has new task", this.name);
                        curAction = tasks.Dequeue();
                        curAction();
                    }
                    else
                    {
                        Console.WriteLine("{0} now wait", name);
                        Monitor.Wait(tasks);

                    }
                }
            }
        }

        public void Stop()
        {
            curThread.Join();
            Console.WriteLine("ENDEND");
        }
    }
}
