using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab_3_PandC
{
    public class ProducersAndConsuments
    {
        static int maxBufSize = 20;
        static int timeToSleep = 100;
        static Queue<int> Buf = new Queue<int>();
        static bool stopFlag = false;
        static Random rnd = new Random();

        public static void startSimulation(int numOfProducers, int numOfConsuments)
        {
            Queue<Thread> prodThreadList = new Queue<Thread>();
            Queue<Thread> consThreadList = new Queue<Thread>();

            Console.WriteLine("press any key ===========> simulation starts");
            Console.WriteLine("press any key once more => simulation ends");
            Console.ReadKey();

            for (int i = 0; i < numOfProducers; i++)
            {
                Producer producer = new Producer();
                Thread prodThread = new Thread(() => producer.Run());
                prodThread.Name = "prod" + i.ToString();

                prodThreadList.Enqueue(prodThread);
                prodThread.Start();
            }
            for (int i = 0; i < numOfConsuments; i++)
            {
                Consumer consumer = new Consumer();
                Thread consThread = new Thread(() => consumer.Run());
                consThread.Name = "cons" + i.ToString();

                consThreadList.Enqueue(consThread);
                consThread.Start();
            }

            Console.ReadKey();
            stopFlag = true;

            for (int i = 0; i < numOfProducers; i++)
            {
                prodThreadList.Dequeue().Join();
            }
            for (int i = 0; i < numOfConsuments; i++)
            {
                consThreadList.Dequeue().Join();
            }
        }

        class Producer
        {
            public void Run()
            {
                while (!stopFlag)
                {
                    int data = rnd.Next(10, 99);

                    Monitor.Enter(Buf);
                    if (Buf.Count < maxBufSize)
                    {
                        Buf.Enqueue(data);
                        //Console.WriteLine(Thread.CurrentThread.Name + " put " + data.ToString());
                        foreach (int dat in Buf)
                        {
                            Console.Write(dat + " ");
                        }
                        Console.WriteLine();
                    }
                    Monitor.Exit(Buf);

                    Thread.Sleep(timeToSleep);
                }
            }
        }

        class Consumer
        {
            public void Run()
            {
                while (!stopFlag)
                {
                    int data;

                    Monitor.Enter(Buf);
                    if (Buf.Count > 0)
                    {
                        data = Buf.Dequeue();
                        //Console.WriteLine(Thread.CurrentThread.Name + " get " + data.ToString());
                        foreach (int dat in Buf)
                        {
                            Console.Write(dat + " ");
                        }
                        Console.WriteLine();
                    }
                    Monitor.Exit(Buf);
                    Thread.Sleep(timeToSleep);
                }
            }
        }
    }
}