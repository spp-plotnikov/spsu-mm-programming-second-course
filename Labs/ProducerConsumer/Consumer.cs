using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Consumer
    {
        List<int> buff;
        Semaphore sem;
        bool fl = true;
        Thread thread;
        public Consumer(ref List<int> buffer, ref Semaphore semph, int i)
        {
            buff = buffer;
            sem = semph;
            Console.WriteLine(buff.Count);
            thread = new Thread(StartWork);
            thread.Name = "Consumer_" + i;
            thread.Start();
        }

        private void StartWork()
        {
            while (fl)
            {
                //Console.WriteLine("{0} is waiting in QUEUE...", Thread.CurrentThread.Name);
                sem.WaitOne();
                if (buff.Count == 0)
                {
                    sem.Release();
                    continue;
                }
                //Console.WriteLine("{0} enters the Critical Section!", Thread.CurrentThread.Name);
                buff.RemoveAt(0);
                Console.WriteLine("{0} get", Thread.CurrentThread.Name);

                Console.WriteLine(buff.Count);
                //Console.WriteLine("{0} is leaving the Critical Section", Thread.CurrentThread.Name);
                sem.Release();
                Thread.Sleep(1000);
            }
            Console.WriteLine("{0} deleted", Thread.CurrentThread.Name);

        }

        public void Delete()
        {
            fl = false;
            thread.Join();
        }
    }
}
