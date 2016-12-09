using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class Worker
{
    private bool runneble = false;
    private Thread thread;
    private Queue<Action> tasksQueue;

    public Worker(Queue<Action> tasksQueue)
    {
        this.thread = new Thread(Run);
        this.tasksQueue = tasksQueue;
    }

    public void Start()
    {
        this.thread.Start();
        this.runneble = true;
    }

    public void Stop()
    {
        this.runneble = false;
    }

    public void Run()
    {
        // worker loop
        while (runneble)
        {
            Action curTask = new Action(() => { });
            lock (tasksQueue)
            {
                // if queue not empty
                if (tasksQueue.Count() != 0)
                {
                    curTask = tasksQueue.Dequeue();
                    Console.WriteLine("" + Thread.CurrentThread.ManagedThreadId + " worker is working now");
                    // signal to first in queue of threads
                    Monitor.Pulse(tasksQueue);
                }
                else
                {
                    // wait signal
                    Console.WriteLine("" + Thread.CurrentThread.ManagedThreadId + " is waiting now...");
                    Monitor.Wait(tasksQueue);
                }
            }
            // if already stoped 
            if (!runneble)
                return;
            curTask();
        }
    }
}