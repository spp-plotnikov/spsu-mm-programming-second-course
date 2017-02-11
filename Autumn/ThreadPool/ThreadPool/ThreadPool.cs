using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace ThreadPool
{
    public class ThreadPool: IDisposable
    {
        private bool _finish = false;
        private const int NumThread = 3;
        private Queue<Action> taskQueue = new Queue<Action>();
        private object threadLock = new object();
        private Thread[] threadArr = new Thread[3];

        public void Enqueue(Action task)
        {
            lock (threadLock)
            {
                taskQueue.Enqueue(task);
            }
        }

        public void Dispose()
        {
            taskQueue.Clear();
            _finish = true;
            Console.WriteLine("Dispose finished");
        }

        public void Start()
        {
            int _count = taskQueue.Count;
            for (int j = 0; j <= _count; j++)
            {
                threadArr[j % 3] = new Thread(() =>
                {
                    if (taskQueue.Count != 0)
                    {
                        Action act;
                        lock (threadLock)
                        {
                            act = taskQueue.Dequeue();
                        }
                        act();
                    }
                    else
                    {
                        if (!_finish)
                            Thread.Sleep(100);
                    }
                });
                threadArr[j % 3].Start();
            }
        }




    }
}
