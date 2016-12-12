using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab4
{
    class ThreadPool : IDisposable
    {
        ThreadInfo[] _threadsInfo;
        object _locker;
        bool _isFinished;

        public ThreadPool(int numberOfThreads)
        {
            _locker = new object();
            _isFinished = false;
            _threadsInfo = new ThreadInfo[numberOfThreads];
            for (int i = 0; i < numberOfThreads; i++)
            {
                var threadInfo = new ThreadInfo();
                threadInfo.ThreadNeedSteal += Steal;
                _threadsInfo[i] = threadInfo;
            }
        }

        public void Start()
        {
            foreach (var threadInfo in _threadsInfo)
            {
                threadInfo.Thread.Start();
            }
        }

        public void Steal(object sender, EventArgs args)
        {
            var curThreadInfo = (ThreadInfo)sender;
            if (TryToSteal())
            {
                foreach (var threadInfo in _threadsInfo)
                {
                    if (threadInfo != curThreadInfo)
                    {
                        Action action;
                        if (threadInfo.QueueTasks.TryDequeue(out action))
                        {
                            curThreadInfo.Enqueue(action);
                            return;
                        }
                    }
                }
            }
            else
            {
                Thread.Sleep(100);
                return;
            }
        }

        public bool TryToSteal()
        {
            foreach (var threadInfo in _threadsInfo)
            {
                if (threadInfo.QueueTasks.Count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public ThreadInfo GetRandomThread()
        {
            return _threadsInfo[new Random().Next(0, _threadsInfo.Length)];
        }

        public void Dispose()
        {
            lock (_locker)
            {
                foreach (var threadInfo in _threadsInfo)
                {
                    threadInfo.Dispose();
                }
                _isFinished = true;
                Monitor.PulseAll(_locker);
            }
        }

        public void Enqueue(Action a)
        {
            lock (_locker)
            {
                GetRandomThread().Enqueue(a);
                Monitor.PulseAll(_locker);
            }
        }
    }
}
