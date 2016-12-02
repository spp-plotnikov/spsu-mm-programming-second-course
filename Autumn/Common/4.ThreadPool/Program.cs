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
        while(Runneble)
        {
            Action curTask = new Action(() => { });
            lock(TasksQueue)
            {
                // if queue not empty
                if (TasksQueue.Count() != 0)
                {
                    curTask = TasksQueue.Dequeue();
                    Console.WriteLine("I'm " + Thread.CurrentThread.Name + " is working now");
                    Monitor.Pulse(TasksQueue);
                }
                else
                {
                    Console.WriteLine("Waiting...");
                    Monitor.Wait(TasksQueue);
                }
            }
            if (!Runneble)
                return;
            curTask();
        }
    }
}


namespace ThreadPool
{
    class Program
    {
        public static void MyTask()
        {
            Console.WriteLine("executing");
            Thread.Sleep(300);
        }

        static void Main(string[] args)
        {
            Queue<Action> tasksQueue = new Queue<Action>();
           

            Worker th1 = new Worker(tasksQueue);
            th1.Start();
            Worker th2 = new Worker(tasksQueue);
            th2.Start();

            Worker th3 = new Worker(tasksQueue);
            th3.Start();

            Action task = new Action(MyTask);
            Console.ReadKey();
            lock (tasksQueue)
            {
                Monitor.Pulse(tasksQueue);
            }
            tasksQueue.Enqueue(task);

            //tasksQueue.Enqueue(task);
            //tasksQueue.Enqueue(task);

            Console.WriteLine("Done!");
            Console.ReadKey();

            lock (tasksQueue)
            {
                th1.Stop();
                th2.Stop();
                th3.Stop();
                Monitor.PulseAll(tasksQueue);
                Console.WriteLine("all threads have been stopped");
            }

            //Monitor.PulseAll(tasksQueue);
            //th1.Stop();

        }
    }
}
