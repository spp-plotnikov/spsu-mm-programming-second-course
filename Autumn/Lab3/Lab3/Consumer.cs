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

        public Consumer(int id, ConcurrentQueue<int> buffer, Mutex mutex)
        {
            _id = id;
            _mutex = mutex;
            _isFinished = false;
            _buffer = buffer;
            _thread = new Thread(Process);
            _thread.Start();
        }

        public void Process()
        {
            while (!_isFinished)
            {
                if (!_buffer.IsEmpty)
                {
                    _mutex.WaitOne();
                    int item;
                    if (_buffer.TryDequeue(out item))
                    {
                        Console.WriteLine("Consumer {0} take {1}", _id, item);
                    }
                    _mutex.ReleaseMutex();
                    Thread.Sleep(10);
                }
            }
        }

        public void Stop()
        {
            _isFinished = true;
        }
    }
}