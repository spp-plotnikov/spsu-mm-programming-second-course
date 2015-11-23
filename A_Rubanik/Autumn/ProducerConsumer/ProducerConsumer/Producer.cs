using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Producer : Program
    {
        Random MyRandom;
            public Producer(int n)
            {
                MyRandom = new Random(n);
            }
            public void Producing()
            {
                while(true)
                {
                    if (Console.KeyAvailable)
                    {
                        break;
                    }
                    else
                    {
                        int AddingNumber = MyRandom.Next(100);
                        mutex.WaitOne();
                        list.Add(AddingNumber);
                        mutex.ReleaseMutex();
                        Console.WriteLine("I add to list " + AddingNumber);
                        int timeSleep = 100 + MyRandom.Next(1000);

                        Thread.Sleep(timeSleep);
                    }
                }
            }      
    
    }
}
