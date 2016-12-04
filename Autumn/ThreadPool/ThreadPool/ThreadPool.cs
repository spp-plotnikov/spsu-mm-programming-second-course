using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {
        private readonly int _threadCount = 20;
        private List<Thread> _threads;
        private Queue<Action> _tasks; 
        private object _syncObj = new object();
        private bool _isFinished;
         

        public ThreadPool()
        {
            _threads = new List<Thread>(_threadCount);
            for (int i = 0; i < _threadCount; i++)
            {
                _threads.Add(new Thread(Work));
            }
            _tasks = new Queue<Action>();
            _isFinished = false;
        }

        public void Start()
        {
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        private void Work()
        {
            while (!_isFinished)
            {
                Action task;
                lock (_syncObj)
                {

                    while (_tasks.Count == 0 && !_isFinished)
                    {
                        Monitor.Wait(_syncObj);
                    }

                    if (_isFinished)
                        return;

                    task = _tasks.Dequeue();
                }
                task.Invoke();
            }
        }

        public void Enqueue(Action action)
        {
            lock (_syncObj)
            {
                _tasks.Enqueue(action);
                Monitor.PulseAll(_syncObj);
            }
        }

        public void Dispose()
        {
            lock (_syncObj)
            {
                _isFinished = true;
                Monitor.PulseAll(_syncObj);
            }
        }
    }
}