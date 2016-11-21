using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public class ThreadPool : IDisposable
    {
        int numOfThreads_ = 5;
        MyThread[] threads_ = new MyThread[5];
        Queue<Action> poolActions_ = new Queue<Action>();

        public ThreadPool()
        {
            for (int i = 1; i <= numOfThreads_; i++)
            {
                threads_[i - 1] = new MyThread(poolActions_, i);
            }
        }

        public void AddToPool(Action newAction)
        {
            lock (poolActions_)
            {
                poolActions_.Enqueue(newAction);
                int itr = 0;

                // find the thread to do the job
                while (itr < numOfThreads_ && threads_[itr].Working.WaitOne(0) != false)
                {
                    itr++;
                }

                if(itr < numOfThreads_)
                {
                    threads_[itr].Working.Set();
                }

                Monitor.Pulse(poolActions_);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < numOfThreads_; i++)
            {
                threads_[i].Finish();
            }

            Console.WriteLine("All threads deleted");
        }
    }
}
