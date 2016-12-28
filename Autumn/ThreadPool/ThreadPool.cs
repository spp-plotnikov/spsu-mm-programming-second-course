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

        static private Queue<Action>[] _tPool = new Queue<Action>[2];
        static private object _lock;
        static private bool _isFinished;
        static private int _count;
        private MyThread[] _pool = new MyThread[2]; //pool, actually

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadPool(int num)
        {
            NumOfThreads = num;
            Array.Resize<MyThread>(ref _pool, NumOfThreads); // malloc, sort of
            Array.Resize<Queue<Action>>(ref _tPool, NumOfThreads);
            //_actQ = new Queue<Action>();
            _lock = new object();
            _isFinished = false;
            for (int i = 0; i < NumOfThreads; i++) // pool initialisation
            {
                _pool[i] = new MyThread(i);
                _tPool[i] = new Queue<Action>();
            }
        }

        public void Enqueue(Action a)
        {
            lock (_lock)
            {
                //_actQ.Enqueue(a);
                _tPool[_count++ % NumOfThreads].Enqueue(a);
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
            int lenght = 0;
            lock (_lock)
            {
                foreach (var q in _tPool)
                  lenght  += q.Count();
            }
            return lenght == 0;
        }
        /// <summary>
        /// Support class for task/thread  encapsulation
        /// </summary>
        private class MyThread
        {

            private Thread _thread;
            public MyThread(int id)
            {
                _thread = new Thread(() => GetAction(id));
            }

            public void Start()
            {
                _thread.Start();
            }

            private void GetAction(int id)
            {
                Action act = null;
                while (true)
                {
                    lock (_lock)
                    {
                        while (_tPool[id].Count == 0 && !_isFinished)
                        {
                            if (_tPool[id].Count == 0)
                            {
                                int i = 0;
                                while (_tPool[(id + i) % _tPool.Length].Count == 0 && i < _tPool.Length)
                                    i++;
                                if (i < _tPool.Length)
                                {
                                    act = _tPool[(id + i) % _tPool.Length].Dequeue();
                                    break;
                                }

                                //if we are here - we must finish or wait
                                Monitor.Wait(_lock);
                            }
                        }

                        if (_isFinished)
                            return;

                        act = (act == null) ? _tPool[id].Dequeue() : act;
                    }

                    act();
                }
            }
        }

    }
}
