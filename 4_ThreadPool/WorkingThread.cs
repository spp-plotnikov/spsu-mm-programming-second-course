using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class WorkingThread
{
    private Queue<Action> tasks;
    private int threadNumber;
    private Thread thread;
    private bool flag = true;

    public WorkingThread(int threadNumber,  Queue<Action> tasks)
    {
        this.threadNumber = threadNumber;
        this.tasks = tasks;
        this.thread = new Thread(() => Run());
        thread.Start();
    }

    public void Run()
    {
        while (flag)
        {
            Action task = new Action(() => { });
            lock (tasks)
            {
                if(tasks.Count() != 0)
                {
                    task = tasks.Dequeue();
                    Console.WriteLine("Thread{0} is working, num of tasks in the queue - {1}", threadNumber, tasks.Count());
                }
                else
                {
                    Monitor.Wait(tasks);
                    if(flag == false)
                    {
                        return;
                    }
                }
            }
            task();
        }
    }

    public void Close()
    {
        flag = false;
    }

    public void JoinT()
    {
        thread.Join();
    }
}
