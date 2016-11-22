using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public static class Workspace
    {
        // storage for all available things 
        public static List<int> Goods = new List<int>();
        private static bool _working = true;
        private static Random _rnd = new Random();
        public static int NumOfProducers;
        public static int NumOfConsumers;

        public static void DoTheWork()
        {
            ThreadProducer[] prodsThreads = new ThreadProducer[NumOfProducers];
            ThreadConsumer[] consThreads = new ThreadConsumer[NumOfConsumers];

            // creating threads for producers
            for (int i = 0; i < NumOfProducers; i++)
            {
                prodsThreads[i] = new ThreadProducer(i);
            }

            // creating threads for consumers
            for (int i = 0; i < NumOfConsumers; i++)
            {
                consThreads[i] = new ThreadConsumer(i);
            }

            Console.ReadKey(true);
            _working = false;
        }

        private class ThreadProducer
        {
            public Thread thread;
            private readonly int _name;
            public ThreadProducer(int name)
            {
                _name = name;
                thread = new Thread(this.Run);
                thread.Start();
            }

            void Run()
            {
                while (_working) TakeSomething();
            }

            public void TakeSomething()
            {
                int toPut;
                Monitor.Enter(Goods);
                toPut = _rnd.Next(0, 1000);
                Console.WriteLine("Producer number {0} added to list {1}", _name, toPut);
                Goods.Add(toPut);
                Monitor.Pulse(Goods);
                Monitor.Exit(Goods);
                Thread.Sleep(400);
            }
        }

        private class ThreadConsumer
        {
            public Thread thread;
            private readonly int _name;
             
            public ThreadConsumer(int name)
            {
                _name = name;
                thread = new Thread(this.Run);
                thread.Start();
            }

            void Run()
            {
                while (_working) TakeSomething();
            }

            // removes some element from the given list
            public void TakeSomething()
            {
                Monitor.Enter(Goods);

                if (Goods.Count > 0)
                {
                    int toRemove = _rnd.Next(0, Goods.Count - 1);
                    Console.WriteLine("Consumer number {0} removed from list element {1}", _name, Goods[toRemove]);
                    Goods.Remove(Goods[toRemove]);
                }
                else
                {
                    Console.WriteLine("There is nothing to remove from the list!");
                    Monitor.Wait(Goods);
                }

                Monitor.Exit(Goods);
                Thread.Sleep(400);
            }
        }
    }
}
