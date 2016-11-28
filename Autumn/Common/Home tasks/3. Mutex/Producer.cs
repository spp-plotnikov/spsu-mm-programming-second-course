using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_consumer
{
    class Producer
    {
        private int name;
        private Thread thread;
        private bool isWorking;
        private int delay;
        private Buffer buffer;
        
        public Producer(int num, Buffer buf)
        {
            buffer = buf;
            delay = 500;
            name = num;
            thread = new Thread(() => Run());
            isWorking = true;
        }

        public void Run()
        {
            while (isWorking)
            {
                buffer.BufEnque(name);
                Console.WriteLine("Producer " + name + " add " + name + " at top of buffer");
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
            thread.Join();
        }
    }


}
