using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


public class Tasks
{
    private int tasksNum;
    private ThreadPool pool;
    private bool flag = true;
    private Thread tasks;

    public Tasks(ThreadPool pool)
    {
        this.pool = pool;
        this.tasksNum = 0;
        this.tasks = new Thread(() => Run());
        tasks.Start();
    }
    public void Run()
    {
        while (flag)
        {
            Console.WriteLine("New task - {0}!", tasksNum);
            Interlocked.Add(ref tasksNum, 1);
            pool.Enqueue(() =>
            {
                Thread.Sleep(4000);
                Console.WriteLine("Task is done");
            });
            Thread.Sleep(2000);
        }
    }

    public void Close()
    {
        flag = false;
    }
}