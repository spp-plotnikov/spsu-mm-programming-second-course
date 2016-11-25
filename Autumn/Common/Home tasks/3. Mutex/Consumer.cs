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
        private Buffer buffer;

        public Consumer(int num, Buffer buf)
        {
            buffer = buf;
            delay = 500;
            isWorking = true;
            name = num;
            thread = new Thread(() => Run());
        }

        public void Run()
        {
            while (isWorking)
            {
                while (buffer.GetBufSize() < 1 && isWorking)
                {
                    Thread.Sleep(delay);
                }

                int drop = buffer.BufDeque();
                Console.WriteLine("Consumer " + name + " drop " + drop + " at begin of buffer");
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
