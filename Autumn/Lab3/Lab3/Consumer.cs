using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab3
{
    public class Consumer
    {
        Mutex _mutex;
        int _id;
        bool _isFinished;

        public Consumer(int id)
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
                if (buff.Count > 0)
                {
                    try
                    {
                        int item = buff.First();
                        buff.Remove(item);
                        Console.WriteLine("Consumer {0} take {1}", _id, item);
                    }
                    catch (Exception) { }
                }
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
