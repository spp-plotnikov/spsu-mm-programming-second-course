using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab_3_PandC
{
    class Consumer
    {
        bool stopFlag = false;
        Queue<int> buf;

        public Consumer(int num, Queue<int> buf)
        {
            this.buf = buf;
            Thread[] thrs = new Thread[num];
            for (int i = 0; i < num; i++ )
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
                if(buf.Count != 0)
                {
                    buf.Dequeue();
                    foreach (int i in buf)
                    {
                        Console.Write("{0} ", i);
                    }
                    Console.WriteLine();
                    Monitor.Exit(buf);
                    Thread.Sleep(100); //pause between add/del
                }
                else
                {
                    Monitor.Wait(buf);
                    Monitor.Exit(buf);
                }
            }
        }

        public void Stop()
        {
            stopFlag = true;
            Monitor.Enter(buf);
            Monitor.PulseAll(buf);
            Monitor.Exit(buf);
        }
    }
}