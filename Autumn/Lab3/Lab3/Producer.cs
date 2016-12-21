using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab3
{
    public class Producer
    {
        Random _rnd = new Random();
        int _id;
        Mutex _mutex;
        bool _isFinished;
        ConcurrentQueue<int> _buffer;
        Thread _thread;

        public Producer(int id, ConcurrentQueue<int> buffer, Mutex mutex)
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
                int item = _rnd.Next(1, 100);
                _buffer.Enqueue(item);
                Console.WriteLine("Producer {0} add {1}", _id, item);
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