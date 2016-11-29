using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {
        const int numOfThreads = 10;
        private Queue<Action> tasks = new Queue<Action>();
        private List<MyThread> threads = new List<MyThread>();

        public int GetNumOfTask()
        {
            return tasks.Count;
        }

        public void Enqueue (Action task)
        {
            lock (tasks)
           {
                Monitor.Enter(tasks);
                tasks.Enqueue(task);
                Monitor.PulseAll(tasks);
                Monitor.Exit(tasks);
            }
        }
        
        public void Start()
        {
            for (int i = 0; i < numOfThreads; i++)
            {
                MyThread thread = new MyThread(tasks, i);
                threads.Add(thread);
            }
        }

        public void Dispose()
        {
            lock (tasks)
            {
                tasks.Clear();
                for (int i = 0; i < numOfThreads; i++)
                {
                    threads[i].SetWork();
                }
                for (int i = 0; i < numOfThreads; i++)
                {
                    Monitor.Pulse(tasks);
                }
                for (int i = 0; i < numOfThreads; i++)
                {
                    threads[i].Stop();
                }
                Monitor.Pulse(tasks);
                Console.WriteLine("all threads have been stopped");
            }
        }
    }
}
