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
        public static Queue<Action> PoolActions = new Queue<Action>();

        public ThreadPool()
        {
            for (int i = 0; i < numOfThreads_; i++)
            {
                threads_[i] = new MyThread();
            }
        }

        public void AddToPool(Action newAction)
        {
            lock (PoolActions)
            {
                PoolActions.Enqueue(newAction);
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

                Monitor.Pulse(PoolActions);
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

        private class MyThread
        {
            Thread mySubThread_;
            bool work_ = true;
            public ManualResetEvent Finished = new ManualResetEvent(false);
            public ManualResetEvent Working = new ManualResetEvent(false);

            public MyThread()
            {
                mySubThread_ = new Thread(MyThreadStart);
                mySubThread_.Start();
            }

            public void MyThreadStart()
            {
                while (work_)
                {
                    // prevent from other threads changes of the tasks queue
                    Monitor.Enter(PoolActions);

                    // if there is something to do
                    if (PoolActions.Count > 0)
                    {
                        Action myCurAction = PoolActions.Dequeue();
                        myCurAction();
                        Console.WriteLine("The task is completed");
                        Console.WriteLine();
                        Console.WriteLine("---------------------------------------------------");
                        Monitor.Exit(PoolActions);
                    }
                    else
                    {
                        // don't need to change the tasks queue, finish blocking immediately
                        Monitor.Exit(PoolActions);

                        // waiting for some new job or to finish the thread
                        Working.Reset();
                        Finished.Set();
                        Working.WaitOne();
                        Finished.Reset();
                    }
                }
            }

            // end the thread
            public void Finish()
            {
                work_ = false;
                Finished.WaitOne();
                Working.Set();
                mySubThread_.Join();
            }
        }
    }
}
