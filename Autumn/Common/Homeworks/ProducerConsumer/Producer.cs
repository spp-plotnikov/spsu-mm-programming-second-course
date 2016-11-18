using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Producer
    {
        readonly int name_;

        public Producer(int name)
        {
            this.name_ = name;
        }

        // adds some element to given list
        public void DoSomething(bool working, ref List<int> goods)
        {
            int toPut;
            lock (goods)
            {
                if(!working)
                {
                    Monitor.Pulse(goods);
                    return;
                }
                toPut = new Random().Next(0, 1000);
                Console.WriteLine("Producer number {0} added to list {1}", name_, toPut);
                goods.Add(toPut);

                Thread.Sleep(200);
                Monitor.Pulse(goods);
                Monitor.Wait(goods);
            }
        }
    }
}
