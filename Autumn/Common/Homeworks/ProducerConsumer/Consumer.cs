using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Consumer
    {
        private readonly int name_;

        public Consumer(int name)
        {
            this.name_ = name;
        }

        // removes some element from the given list
        public void DoSomething(bool working, ref List<int> goods)
        {
            lock (goods)
            {
                if(!working)
                {
                    Monitor.Pulse(goods);
                    return;
                }

                if(goods.Count > 0)
                {
                    int toRemove = new Random().Next(0, goods.Count - 1);
                    Console.WriteLine("Consumer number {0} removed from list element {1}", name_, goods[toRemove]);
                    goods.Remove(goods[toRemove]);
                }
                else Console.WriteLine("There is nothing to remove from the list!");

                Thread.Sleep(200);
                Monitor.Pulse(goods);
                Monitor.Wait(goods);
            }
        }
    }
}
