using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab_3_PandC
{
    class Producer
    {
        Random rnd = new Random();
        bool stopFlag = false;
        Queue<int> buf;

        public Producer(int num, Queue<int> buf)
        {
            this.buf = buf;
            Thread[] thrs = new Thread[num];
            for(int i = 0; i < num; i++)
            {
                thrs[i] = new Thread(Act);
                thrs[i].Start();
            }
        }

        void Act()
        {
            while(!stopFlag)
            {
                Monitor.Enter(buf);
                buf.Enqueue(rnd.Next(10, 99));
                foreach(int i in buf)
                {
                    Console.Write("{0} ", i);
                }
                Console.WriteLine();
                Monitor.Exit(buf);

                Thread.Sleep(100);  //pause between add/del
            }
        }

        public void Stop()
        {
            stopFlag = true;
        }
    }
}