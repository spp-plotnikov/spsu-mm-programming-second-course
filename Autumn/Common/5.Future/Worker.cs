using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class Worker
{
    private bool Runneble = false;
    private Thread Thread;
    private Queue<Action> TasksQueue;

    public Worker(Queue<Action> tasksQueue)
    {
        this.Thread = new Thread(Run);
        this.TasksQueue = tasksQueue;
    }

    public void Start()
    {
        this.Thread.Start();
        this.Runneble = true;
    }

    public void Stop()
    {
        this.Runneble = false;
    }

    public void Run()
    {
        // worker loop
        while (Runneble)
        {
            Action curTask = new Action(() => { });
            lock (TasksQueue)
            {
                // if queue not empty
                if (TasksQueue.Count() != 0)
                {
                    curTask = TasksQueue.Dequeue();
                    Console.WriteLine("" + Thread.CurrentThread.ManagedThreadId + " worker is working now");
                    // signal to first in queue of threads
                    Monitor.Pulse(TasksQueue);
                }
                else
                {
                    // wait signal
                    Console.WriteLine("" + Thread.CurrentThread.ManagedThreadId + " is waiting now...");
                    Monitor.Wait(TasksQueue);
                }
            }
            // if already stoped 
            if (!Runneble)
                return;
            curTask();
        }
    }
}