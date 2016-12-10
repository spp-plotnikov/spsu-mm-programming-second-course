using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Threadpool main class
/// </summary>
namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {       
        public int NumOfThreads
        {
           get;
            private set;            
        }

        static private Queue<Action> _actQ;
        static private object _lock;  
        static private bool _isFinished;
        private MyThread[] _pool= new MyThread[2]; //pool, actually

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadPool(int num)
        {
            NumOfThreads = num;
            Array.Resize<MyThread>(ref _pool, NumOfThreads); // malloc, sort of

            _actQ = new Queue<Action>();
            _lock = new object(); 
            _isFinished = false;
            for (int i = 0; i < NumOfThreads; i++) // pool initialisation
            {
                _pool[i] = new MyThread();
            }
        }

        public void Enqueue(Action a)
        {
            lock (_lock)
            {
                _actQ.Enqueue(a);
                Monitor.PulseAll(_lock);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _isFinished = true;
                Monitor.PulseAll(_lock);
            }
        }

        public void Start()
        {
            foreach (MyThread thread in _pool)
            {
                thread.Start();
            }
        }

        public bool IsFinished()
        {
            int lenght;
            lock (_lock)
            {
                lenght = _actQ.Count();
            }
            return lenght == 0;
        }
        /// <summary>
        /// Support class for task/thread  encapsulation
        /// </summary>
        private class MyThread
        {
            private Thread _thread;
            public MyThread()
            {
                _thread = new Thread(GetAction);
            }

            public void Start()
            {
                _thread.Start();
            }

            private void GetAction()
            {
                Action act;
                while (true)
                {
                    lock (_lock)
                    {
                        while (_actQ.Count == 0 && !_isFinished)
                            Monitor.Wait(_lock);  

                        if (_isFinished)
                            return;

                        act = _actQ.Dequeue();  
                    }
                    act();
                }
            }
        }
    }

}
