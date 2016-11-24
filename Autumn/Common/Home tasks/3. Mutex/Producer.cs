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
        private Additional additional;
        
        public Producer(int num, Additional a)
        {
            additional = a;
            delay = 500;
            name = num;
            thread = new Thread(() => Run());
            isWorking = true;
        }

        public void Run()
        {
            while (isWorking)
            {
                additional.MtxWait();
                Console.WriteLine("Producer " + name + " has bocked mutex");
                additional.BufEnque(name);
                Console.WriteLine("Producer " + name + " add " + name + " at top of buffer");
                additional.MtxRelease();
                Console.WriteLine("Producer " + name + " has released mutex");
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
