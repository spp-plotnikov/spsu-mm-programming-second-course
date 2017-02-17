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
        private string name;
        private object obj;
        public delegate void MyDelegate();
        public event MyDelegate IsReady;

        public MyThread(String name)
        {
            tasks = new Queue<Action>();
            thread = new Thread(Run);
            this.name = name;
            obj = new object();
        }

        public void Enqueue(Action task)
        {
            tasks.Enqueue(task);
            if(!isWorking)
            {
                isWorking = true;
                Start();
            }
        }

        public void Run()
        {
            while(isWorking)
            {
                Action onGoing = new Action(() => { });
                lock(obj)
                {
                    if(tasks.Count() != 0)
                    {
                        onGoing = tasks.Dequeue();
                        Console.WriteLine(name + " is running.");
                    }
                    else
                    {
                        IsReady();
                        isWorking = false;
                    }
                    Monitor.PulseAll(obj);
                }
                if(!isWorking)
                {
                    return;
                }
                onGoing();
            }
        }

        public Action Give()
        {
            lock(obj)
            {
                if(tasks.Count != 0)
                {
                    Action task = tasks.Dequeue();
                    Monitor.PulseAll(obj);
                    return task;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Start()
        {
            isWorking = true;
            try
            {
                thread.Start();
            }
            catch
            {
                thread = new Thread(Run);
                thread.Start();
            }
        }

        public void Dispose()
        {
            lock(obj)
            {
                isWorking = false;
                tasks.Clear();
                Monitor.PulseAll(obj);
            }
            return;
        }
    }
}
