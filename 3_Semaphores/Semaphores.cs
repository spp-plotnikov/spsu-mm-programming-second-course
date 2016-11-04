using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducerVsConsumer
{
    public class Semaphores
    {
        private static int wait = 1000;
        private static bool flag = true;

        public static List<int> Buf = new List<int>();

        private static int numProd = 6;
        private static int numCons = 3;
        public static List<Thread> ProducersList = new List<Thread>();
        public static List<Thread> ConsumersList = new List<Thread>();
        
        public static Semaphore Critical = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("Press key to stop this prog");
            Thread.Sleep(wait * 2);
            for (int i = 0; i < numProd; i++)
            {
                Producer producerObj = new Producer(i);
                Thread prod = new Thread(() => producerObj.Run());
                ProducersList.Add(prod);
                prod.Start();
            }

            for (int i = 0; i < numCons; i++)
            {
                Consumer consumerObj = new Consumer(i);
                Thread cons = new Thread(() => consumerObj.Run());
                ConsumersList.Add(cons);
                cons.Start();
            }
            Console.ReadLine();
            Console.WriteLine("All processes will be finished, please, wait");
            flag = false;
            for (int i = 0; i < numCons; i++)
            {
                ConsumersList[0].Join();
                ConsumersList.RemoveAt(0);
            }
            for (int i = 0; i < numProd; i++)
            {
                ProducersList[0].Join();
                ProducersList.RemoveAt(0);
            }
            Console.WriteLine("The end");
            Console.ReadLine();
        }

        public class Producer
        {
            private readonly int name;

            public Producer(int name)
            {
              this.name = name;

            }

            public void Run()
            {
                while (flag)
                {
                    Critical.WaitOne();
                    int item = name;
                    Buf.Add(item);
                    Console.WriteLine("Producer №{0} add the item = {1}", name, item);
                    Thread.Sleep(wait);
                    Critical.Release();
                }
            }
        }

        public class Consumer
        {
            private readonly int name;

            public Consumer(int name)
            {
                this.name = name;

            }

            public void Run()
            {
                while(flag)
                {
                    Critical.WaitOne();
                    if(Buf.Count() > 0)
                    {
                        int item = Buf.Last();
                        Buf.Remove(item);
                        Console.WriteLine("Consumer №{0} get the item = {1}", name, item);
                        Thread.Sleep(wait);
                        Critical.Release();
                    }
                }
            }
        }
    }
}
