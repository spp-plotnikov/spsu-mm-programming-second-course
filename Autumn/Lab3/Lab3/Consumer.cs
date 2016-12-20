using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab3
{
    public class Consumer
    {
        Mutex _mutex;
        int _id;
        bool _isFinished;
        ConcurrentQueue<int> _buffer;
        Thread _thread;

        public Consumer(int id, ConcurrentQueue<int> buffer)
        {
            _id = id;
            _mutex = new Mutex();
            _isFinished = false;
            _buffer = buffer;
            _thread = new Thread(Process);
            _thread.Start();
        }

        public void Process()
        {
            while (!_isFinished)
            {
                _mutex.WaitOne();
                if (_buffer.Count > 0)
                {
                    int item;
                    if (_buffer.TryPeek(out item))
                    {
                        Console.WriteLine("Consumer {0} take {1}", _id, item);
                    }
                    _mutex.ReleaseMutex();
                    Thread.Sleep(100);
                }
            }
        }

        public void Stop()
        {
            _isFinished = true;
        }
    }
}