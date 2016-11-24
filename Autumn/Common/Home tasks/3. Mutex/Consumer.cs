using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_consumer
{
    class Consumer
    {
        private int name;
        private Thread thread;
        private int delay;
        private bool isWorking;
        private Additional additional;
        public Consumer(int num, Additional a)
        {
            additional = a;
            delay = 500;
            isWorking = true;
            name = num;
            thread = new Thread(() => Run());
        }

        public void Run()
        {
            while (isWorking)
            {
                while (additional.GetBufSize() < 1 && isWorking)
                {
                    Thread.Sleep(delay);
                }
                additional.MtxWait();
                if (additional.GetBufSize() < 1 || !isWorking)
                {
                    additional.MtxRelease();
                    continue;
                }
                Console.WriteLine("Consumer " + name + " has bocked mutex");
                int drop = additional.BufDeque();
                Console.WriteLine("Consumer " + name + " drop " + drop + " at begin of buffer");
                additional.MtxRelease();
                Console.WriteLine("Consumer " + name + " has released mutex");
                Thread.Sleep(delay);
            }
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            isWorking = false;
        }

        public void ThreadJoin()
        {
            thread.Join();
        }
    }
}
