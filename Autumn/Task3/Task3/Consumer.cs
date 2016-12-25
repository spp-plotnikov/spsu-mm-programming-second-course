using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Task3
{
    class Consumer : Worker
    {
        public Consumer(AtomicLock locker, List<int> data, int sleep) : base(locker, data, sleep)
        {

        }
        protected override void Run(AtomicLock locker)
        {
            while(isRunning)
            {
                locker.Capture();
                if(isRunning)
                {
                    if(data.Count == 0)
                    {
                        Console.WriteLine("Nothing to consume.");
                    }
                    else
                    {
                        int value = data.Last();
                        data.Remove(value);
                        Console.WriteLine("Consumed " + value);
                    }
                }
                locker.Release();
                Thread.Sleep(sleep);
            }
        }
    }
}
