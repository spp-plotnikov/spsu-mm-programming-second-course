using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Producer_consumer
{
    class Mutexes
    {
        public static int Delay = 500;
        public static bool IsWorking = true;
        public static Mutex Mtx = new Mutex();
        public static List<int> Buf = new List<int>();
    }

    class Producer
    {
        private int name;
        public Producer(int num)
        {
            name = num;
        }

        public void Run()
        {
            while (Mutexes.IsWorking)
            {
                Mutexes.Mtx.WaitOne();
                Console.WriteLine("Producer " + name + " has bocked mutex");
                Mutexes.Buf.Add(name);
                Console.WriteLine("Producer " + name + " add " + name + " at top of buffer");
                Mutexes.Mtx.ReleaseMutex();
                Console.WriteLine("Producer " + name + " has released mutex");
                Thread.Sleep(Mutexes.Delay);
            }
        }
    }

    class Consumer
    {
        private int name;
        public Consumer(int num)
        {
            name = num;
        }

        public void Run()
        {
            while (Mutexes.IsWorking)
            {
                while (Mutexes.Buf.Count < 1)
                {
                    Thread.Sleep(Mutexes.Delay);
                }
                Mutexes.Mtx.WaitOne();
                Console.WriteLine("Consumer " + name + " has bocked mutex");
                int drop = Mutexes.Buf.First();
                Mutexes.Buf.Remove(drop);
                Console.WriteLine("Consumer " + name + " drop " + drop + " at begin of buffer");
                Mutexes.Mtx.ReleaseMutex();
                Console.WriteLine("Consumer " + name + " has released mutex");
                Thread.Sleep(Mutexes.Delay);
            }
        }
    }

    class Program
    {
        public static List<Thread> ConsumerList = new List<Thread>();
        public static List<Thread> ProducerList = new List<Thread>();
        public static int NumConsumers = 3;
        public static int NumProducers = 3;

        static void Main(string[] args)
        {
            for (int i = 1; i <= NumProducers; i++)
            {
                Producer newProducer = new Producer(i);
                Thread newThread = new Thread(() => newProducer.Run());
                ProducerList.Add(newThread);
                newThread.Start();
            }
            for (int i = 1; i <= NumConsumers; i++)
            {
                Consumer newConsumer = new Consumer(i);
                Thread newThread = new Thread(() => newConsumer.Run());
                ConsumerList.Add(newThread);
                newThread.Start();
            }
            Console.ReadKey();
            Console.WriteLine("\nStopping all threads...");
            Mutexes.IsWorking = false;
            for (int i = 1; i <= NumProducers; i++)
            {
                ProducerList[0].Join();
                ProducerList.RemoveAt(0);
            }
            for (int i = 1; i <= NumConsumers; i++)
            {
                ConsumerList[0].Join();
                ConsumerList.RemoveAt(0);
            }
            Console.WriteLine("All process was stopped");
            Console.ReadKey();
        }
    }
}
