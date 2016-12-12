using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab3
{
    public class Producer
    {
        Random _rnd = new Random();
        int _id;
        Mutex _mutex;
        bool _isFinished;

        public Producer(int id)
        {
            _id = id;
            _mutex = new Mutex();
            _isFinished = false;
        }

        public void Process(ref List<int> buff)
        {
            while (!_isFinished)
            {
                _mutex.WaitOne();
                int item = _rnd.Next(1, 100);
                buff.Add(item);
                Console.WriteLine("Producer {0} add {1}", _id, item);
                _mutex.ReleaseMutex();
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            _isFinished = true;
        }
    }
}
