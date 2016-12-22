using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Lab3
{
    public class Consumer
    {
        Mutex _mutex;
        int _id;
        bool _isFinished;
        Queue<int> _buffer;
        Thread _thread;

        public Consumer(int id, Queue<int> buffer, Mutex mutex)
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
                _mutex.WaitOne();
                if (_buffer.Count > 0)
                {
                    int item = _buffer.Dequeue();
                    Console.WriteLine("Consumer {0} take {1}", _id, item);
                }
                _mutex.ReleaseMutex();
                Thread.Sleep(10);
            }
        }

        public void Stop()
        {
            _isFinished = true;
        }
    }
}