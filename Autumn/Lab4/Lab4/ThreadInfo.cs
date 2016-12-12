using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;

namespace Lab4
{
    class ThreadInfo
    {
        public ConcurrentQueue<Action> QueueTasks;
        public Thread Thread;
        bool _isFinished;
        public event PropertyChangedEventHandler ThreadNeedSteal;

        public ThreadInfo()
        {
            QueueTasks = new ConcurrentQueue<Action>();
            Thread = new Thread(Do);
            _isFinished = false;
        }

        public bool IsEmpty
        {
            get
            {
                if (QueueTasks.Count == 0)
                {
                    ThreadNeedSteal?.Invoke(null, null);
                    return true;
                }
                return false;
            }
        }

        public void Do()
        {
            while (!_isFinished)
            {
                if (_isFinished)
                {
                    return;
                }
                Action action;
                if (QueueTasks.TryDequeue(out action))
                {
                    action();
                }
                else
                {
                    ThreadNeedSteal?.Invoke(this, null);
                }
            }
        }

        public void Enqueue(Action a)
        {
            QueueTasks.Enqueue(a);
        }

        public void Dispose()
        {
            while (QueueTasks.Count > 0)
            {
                Action action;
                if (QueueTasks.TryDequeue(out action))
                {
                    action();
                }
            }
            _isFinished = true;
            Thread = null;
        }
    }
}
