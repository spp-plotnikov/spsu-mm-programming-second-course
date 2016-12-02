using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{

    class Producer
    {
        Thread Thread;
        List<int> sharedData;
        bool Runnable = false;
        private const int pause = 1000;

        public Producer(List<int> sharedData)
        {
            this.sharedData = sharedData;
            this.Runnable = true;
            this.Thread = new Thread(Run);
            this.Thread.Start();
        }

        public void Stop()
        {
            this.Runnable = false;
            this.Thread.Join();
        }

        public void Run()
        {
            while (this.Runnable)
            {
                Locker.Lock();
                // Stil running ? 
                if (this.Runnable)
                {
                    // pushing random value to shared data
                    Random rand = new Random();
                    int value = rand.Next(10, 100);
                    this.sharedData.Add(value);
                    Console.WriteLine("Producer: added value " + value);
                }
                Locker.Release();
                Thread.Sleep(pause);
            }

        }

    }

}
