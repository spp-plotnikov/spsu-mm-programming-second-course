using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class ThreadPool: IDisposable
    {
        const int NumThread = 3;

        private Queue<Action> _taskQueue = new Queue<Action>();
        private Thread[] _threadArr = new Thread[3];
        private int _numPortion = 0;
        private bool _portion = false;
        private bool _finish = false;
        private object _threadLock = new object();

        private struct NewPortion
        {
            public bool portion;
            public int numPortion;
            
            public NewPortion(bool portion, int numPortion)
            {
                this.portion = portion;
                this.numPortion = numPortion;
            }
        }

        NewPortion threadPoolPortion = new NewPortion(false, 0);

        public void Enqueue(Action task)
        {
            lock (_threadLock)
            {
                _taskQueue.Enqueue(task);
                threadPoolPortion.portion = true;
                threadPoolPortion.numPortion += 1;
            }
        }

        public void Dispose()
        {
            _taskQueue.Clear();
            _finish = true;
            Console.WriteLine("Dispose finished");
        }

        public void Start()
        {
            int j = 0;
            while(!_finish)
            {
                if (_taskQueue.Count != 0)
                {
                    _threadArr[j % 3] = new Thread(() =>
                    {
                        _numPortion = threadPoolPortion.numPortion;
                        _portion = threadPoolPortion.portion;
                        Action act = () => { };
                        lock (_threadLock)
                        {
                            if (_taskQueue.Count > 0)
                                act = _taskQueue.Dequeue();
                        }
                        if (_taskQueue.Count > 0)
                            act();
                    });

                    if (_finish)
                        break;

                    _threadArr[j % 3].Start();
                    j++;
                }
                else
                {
                    lock (_threadLock)
                    {
                        if (_numPortion == threadPoolPortion.numPortion)
                            _portion = false;
                    }

                    if (_threadArr[1] == Thread.CurrentThread)
                        while (!_portion)
                            Thread.Sleep(1);
                    else
                        break;

                    if (_finish)
                        break;
                }
            }
        }
    }
}
