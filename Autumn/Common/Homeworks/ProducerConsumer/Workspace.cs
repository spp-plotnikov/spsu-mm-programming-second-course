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
        
        // storages for producers and consumers respectively 
        public static List<Producer> Producers = new List<Producer>();
        public static List<Consumer> Consumers = new List<Consumer>();

        class ThreadProducer
        {
            public Thread thread;
            public bool State;
            public Producer MyProducer;

            public ThreadProducer(bool status, Producer myProd)
            {
                this.State = status;
                this.MyProducer = myProd;
                thread = new Thread(this.Run);
                thread.Start();
            }

            void Run()
            {
                while (State) MyProducer.DoSomething(true, ref Goods);
            }
        }

        class ThreadConsumer
        {
            public Thread thread;
            public bool State;
            public Consumer MyConsumer;

            public ThreadConsumer(bool status, Consumer myCons)
            {
                this.State = status;
                this.MyConsumer = myCons;
                thread = new Thread(this.Run);
                thread.Start();
            }

            void Run()
            {
                while (State) MyConsumer.DoSomething(true, ref Goods);
            }
        }

        public static void DoTheWork()
        {
            ThreadProducer[] prodsThreads = new ThreadProducer[Producers.Count];
            ThreadConsumer[] consThreads = new ThreadConsumer[Consumers.Count];

            // creating threads for producers
            for (int i = 0; i < Producers.Count; i++)
            {
                prodsThreads[i] = new ThreadProducer(true, Producers[i]);
            }

            // creating threads for consumers
            for (int i = 0; i < Consumers.Count; i++)
            {
                consThreads[i] = new ThreadConsumer(true, Consumers[i]);
            }

            Console.ReadKey(true);

            // finish the work
            foreach (ThreadConsumer thread in consThreads)
            {
                thread.State = false;
            }

            foreach (ThreadProducer thread in prodsThreads)
            {
                thread.State = false;
            }

            Console.ReadKey();
        }
    }
}
