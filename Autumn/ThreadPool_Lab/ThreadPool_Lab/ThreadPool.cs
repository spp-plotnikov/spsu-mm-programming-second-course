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
        static private Mutex _queueMutex;
        static private bool _notFinished;
        private MyThread[] _threads = new MyThread[NumOfThreads];
        public ThreadPool ()
        {
            _actionQueue = new Queue<Action>();
            _queueMutex = new Mutex();
            _notFinished = true;
            for (int i = 0; i < NumOfThreads; i++)
            {
                _threads[i] = new MyThread();
            }
        }

        public void Enqueue (Action a)
        {
            _queueMutex.WaitOne();
            _actionQueue.Enqueue(a);
            _queueMutex.ReleaseMutex();
        }

        public void Dispose()
        {
            _notFinished = false;
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
            _queueMutex.WaitOne();
            lenght = _actionQueue.Count();
            _queueMutex.ReleaseMutex();
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
                Action act = null;
                bool smtToDo;
                while (_notFinished)
                {
                    _queueMutex.WaitOne();
                    if (_actionQueue.Count != 0)
                    {
                        act = _actionQueue.Dequeue();
                        smtToDo = true;
                    }
                    else
                        smtToDo = false;
                    _queueMutex.ReleaseMutex();
                    if (smtToDo)
                    {
                        act();
                    }
                }
            }
        }
    }
}