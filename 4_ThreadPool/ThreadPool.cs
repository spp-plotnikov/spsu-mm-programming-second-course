using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

public class ThreadPool : IDisposable
{
    private int poolSize = 1;
    private Queue<Action> tasks;
    private List<WorkingThread> threads = new List<WorkingThread>();

    public ThreadPool()
    {
        this.tasks = new Queue<Action>();
        this.threads = new List<WorkingThread>();
    }

    public void Enqueue(Action task)
    {
        lock(tasks)
        {
            tasks.Enqueue(task);
        }
    }

    public void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
           WorkingThread thread = new WorkingThread(i,  tasks);
           threads.Add(thread);
        }
    }

    public void Dispose()
    {
        lock (tasks)
        {
            tasks.Clear();
            foreach(WorkingThread thread in threads)
            {
                thread.Close();
            }
        }
    }
}
 
