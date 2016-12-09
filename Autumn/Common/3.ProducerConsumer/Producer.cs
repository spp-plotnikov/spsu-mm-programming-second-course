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
        private Thread thread;
        private List<int> sharedData;
        private bool runnable = false;
        private const int pause = 1000;

        public Producer(List<int> sharedData, Locker lockFlag)
        {
            this.sharedData = sharedData;
            this.runnable = true;
            this.thread = new Thread(() => Run(lockFlag));
            this.thread.Start();
        }

        public void Stop()
        {
            this.runnable = false;
            this.thread.Join();
        }

        public void Run(Locker lockFlag)
        {
            while (this.runnable)
            {
                lockFlag.Lock();
                // Stil running ? 
                if (this.runnable)
                {
                    // pushing some value to shared data
                    this.sharedData.Add(24);
                    Console.WriteLine("Producer: added value 24");
                }
                lockFlag.Release();
                Thread.Sleep(pause);
            }

        }

    }

}
