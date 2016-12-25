using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    public class MyThread
    {
        private Queue<Action> tasks;
        private bool isWorking;
        private Thread thread;
        private String name;
        public int TaskCounter
        {
            get;
            private set;
        }
        public delegate void MyDelegate();
        public event MyDelegate IsReady;

        public MyThread(String name)
        {
            //this.tasks = tasks;
            thread = new Thread(Run);
            this.name = name;
            TaskCounter = 0;
        }

        public void Enqueue(Action task)
        {
            tasks.Enqueue(task);
            TaskCounter++;
        }

        public void Run()
        {
            while(isWorking)
            {
                Action onGoing = new Action(() => { });
                lock(tasks)
                {
                    if(tasks.Count() != 0)
                    {
                        onGoing = tasks.Dequeue();
                        Console.WriteLine(name + "is running.");
                    }
                    else
                    {
                        Console.WriteLine(name + "is not busy. Trying to steal.");
                        IsReady();
                    }
                }
                if(!isWorking)
                {
                    return;
                }
                onGoing();
                TaskCounter--;
            }
        }

        public Action Give()
        {
            lock(tasks)
            {
                isWorking = false;
                try
                {
                    Action task = tasks.Dequeue();
                    TaskCounter--;
                    isWorking = true;
                    Monitor.PulseAll(tasks);
                    return task;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void Start()
        {
            thread.Start();
            isWorking = true;
        }
        public void Stop()
        {
            isWorking = false;
        }
        public void Dispose()
        {
            lock(tasks)
            {
                Stop();
                tasks.Clear();
                TaskCounter = 0;
                Monitor.PulseAll(tasks);
            }
            return;
        }
    }
}
