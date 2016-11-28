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
        int _numOfThreads = 5;
        MyThread[] _threads = new MyThread[5];
        public static Queue<Action> PoolActions = new Queue<Action>();

        public ThreadPool()
        {
            for (int i = 0; i < _numOfThreads; i++)
            {
                _threads[i] = new MyThread();
            }
        }

        public void AddToPool(Action newAction)
        {
            Monitor.Enter(PoolActions);
            PoolActions.Enqueue(newAction);
            int itr = 0;

            // find the thread to do the job
            while (itr < _numOfThreads && _threads[itr].Busy != false)
            {
                itr++;
            }

            if(itr < _numOfThreads)
            {
                _threads[itr].GiveSignal();
            }

            Monitor.Exit(PoolActions);
        }

        public void Dispose()
        {
            for (int i = 0; i < _numOfThreads; i++)
            {
                _threads[i].Finish();
            }

            Console.WriteLine("All threads deleted");
        }

        private class MyThread
        {
            private Thread _mySubThread;
            private bool _work = true;
            private ManualResetEvent _finished = new ManualResetEvent(false);
            private ManualResetEvent _working = new ManualResetEvent(false);

            public MyThread()
            {
                _mySubThread = new Thread(MyThreadStart);
                _mySubThread.Start();
            }

            public bool Busy
            {
                get
                {
                    return _working.WaitOne(0);
                }
            }

            public void GiveSignal()
            {
                _working.Set();
            }

            public void MyThreadStart()
            {
                while (_work)
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
                        _working.Reset();
                        _finished.Set();
                        _working.WaitOne();
                        _finished.Reset();
                    }
                }
            }

            // end the thread
            public void Finish()
            {
                _work = false;
                _finished.WaitOne();
                _working.Set();
                _mySubThread.Join();
            }
        }
    }
}
