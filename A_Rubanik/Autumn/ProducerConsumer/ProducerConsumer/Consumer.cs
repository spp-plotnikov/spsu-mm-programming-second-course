using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Consumer : Program
    {
        Random MyRandom;

        public Consumer(int n)
        {
            MyRandom = new Random(n);
        }
        public void Consumering()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    break;
                }
                else
                {
                    int timeSleep = 100 + MyRandom.Next(1000);
                    mutex.WaitOne();
                    if (list.Count == 0)
                    {
                        mutex.ReleaseMutex();
                        Thread.Sleep(timeSleep);
                    }
                    else
                    {
                        int TakingNumber  = list.Last();
                        list.Remove(list.Last());
                        mutex.ReleaseMutex();
                        Console.WriteLine("I take from list " + TakingNumber);
                        Thread.Sleep(timeSleep);
                    }
                }
            }
        }
    }
}
