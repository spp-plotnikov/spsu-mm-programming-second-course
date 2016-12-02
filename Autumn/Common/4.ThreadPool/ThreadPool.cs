using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ThreadPool
{
    class ThreadPool : IDisposable
    {
        const int workersNum = 5;
        private Queue<Action> tasksQueue = new Queue<Action>();
        private List<Worker> workers = new List<Worker>();

        public ThreadPool()
        {
            for (int i = 0; i < workersNum; ++i)
            {
                Worker worker = new Worker(tasksQueue);
                workers.Add(worker);
                worker.Start();
            }
        }

        public void Enqueue(Action task)
        {
            lock (tasksQueue)
            {
                // push task
                tasksQueue.Enqueue(task);
                // push to threads queue
                Monitor.Pulse(tasksQueue);
            }
        }

        public void Dispose()
        {
            lock (tasksQueue)
            {
                tasksQueue.Clear();
                for (int i = 0; i < workersNum; ++i)
                    workers[i].Stop();

                Monitor.PulseAll(tasksQueue);
            }
        }
    }
}
