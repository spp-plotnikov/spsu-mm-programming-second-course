using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool_Lab
{
    public class ThreadPool: IDisposable
    {
        public const int NumOfThreads = 5;
        static private Queue<Action> _actionQueue;
        static private object _lock;
        static private bool _notFinished;
        private MyThread[] _threads = new MyThread[NumOfThreads];

        public ThreadPool ()
        {
            _actionQueue = new Queue<Action>();
            _lock = new object();
            _notFinished = true;
            for (int i = 0; i < NumOfThreads; i++)
            {
                _threads[i] = new MyThread();
            }
        }

        public void Enqueue(Action a)
        {
            lock (_lock)
            {
                _actionQueue.Enqueue(a);
                Monitor.PulseAll(_lock);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _notFinished = false;
                Monitor.PulseAll(_lock);
            }
        }

        public void Start()
        {
            foreach (MyThread thread in _threads)
            {
                thread.Start();
            }
        }

        public bool IsFinished ()
        {
            int lenght;
            lock (_lock)
            {
                lenght = _actionQueue.Count();
            }
            return lenght == 0;
        }

        private class MyThread
        {
            private Thread _thread;
            public MyThread ()
            {
                _thread = new Thread(getAction);
            }

            public void Start ()
            {
                _thread.Start();
            }

            private void getAction ()
            {
                Action act;
                while (true)
                {
                    lock (_lock)
                    {
                        while (_actionQueue.Count == 0 && _notFinished)
                            Monitor.Wait(_lock);

                        if (!_notFinished)
                            return;

                        act = _actionQueue.Dequeue();
                    }
                    act();
                }
            }
        }
    }
}