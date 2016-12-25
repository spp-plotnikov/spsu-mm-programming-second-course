using System;
using System.Collections.Generic;
using System.Threading;

namespace Task3
{
    class Producer : Worker
    {
        public Producer(AtomicLock locker, List<int> data, int sleep) : base(locker, data, sleep)
        {

        }
        
        protected override void Run(AtomicLock locker)
        {
            while(isRunning)
            {
                locker.Capture();
                if(isRunning)
                {
                    int value = new Random().Next() % 100;
                    data.Add(value);
                    Console.WriteLine("Producer added {0}", value);
                }
                locker.Release();
                Thread.Sleep(sleep);
            }
        }
    }
}
