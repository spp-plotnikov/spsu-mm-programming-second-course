using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("press any key for end:");

            const int numOfProd = 3;
            const int numOfCons = 3;
            const int wait = 1000;

            int isLocked = 0; //0 - free
            bool isEnd = false;

            List<int> sharedList = new List<int>();
            List<Thread> currentProducer = new List<Thread>();
            List<Thread> currentConsumer = new List<Thread>();

            
            for (int i = 0; i < numOfProd; i++)
            {
                Producer prod = new Producer(i + 1);
                Thread newThread = new Thread(() => prod.Run(ref isEnd, ref isLocked, ref sharedList, wait));
                currentProducer.Add(newThread);
                newThread.Start();
            }

            for (int i = 0; i < numOfCons; i++)
            {
                Consumer cons = new Consumer(i + 1);
                Thread newThread = new Thread(() => cons.Run(ref isEnd, ref isLocked, ref sharedList, wait));
                currentConsumer.Add(newThread);
                newThread.Start();
            }
            Console.ReadKey();

            while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0))
            { }

            Console.WriteLine("that's all");

            isEnd = true;

            for (int i = 0; i < numOfCons; i++)
            {
                currentConsumer[0].Join();
                currentConsumer.RemoveAt(0);
            }
            for (int i = 0; i < numOfProd; i++)
            {  
                currentProducer[0].Join();
                currentProducer.RemoveAt(0);
            }

            return;
        }


    }

    class Producer
    {
        int name;

        public Producer(int name)
        {
            this.name = name;
        }

        public void Run(ref bool isEnd, ref int isLocked, ref List<int> sharedList, int wait)
        {
            while (!isEnd)
            {
                while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0))
                { }
                Random rand = new Random();
                int val = rand.Next(100);
                sharedList.Add(val);
                Console.WriteLine("Producer # {0} add number {1} to list", name, val);
                Interlocked.Exchange(ref isLocked, 0);
                Thread.Sleep(wait);
            }
        }

    }

    class Consumer
    {
        int name;

        public Consumer(int name)
        {
            this.name = name;
        }


        public void Run(ref bool isEnd, ref int isLocked, ref List<int> sharedList, int wait)
        {
            while (!isEnd)
            {
                while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0))
                { }
                if (sharedList.Count == 0)
                {
                    Console.WriteLine("Consumer # {0} can't get number because list is empty", name);
                }
                else
                {
                    int val = sharedList.Last();
                    Console.WriteLine("Consumer # {0} get {1} from list", name, val);
                    sharedList.Remove(val);
                }
                Interlocked.Exchange(ref isLocked, 0);
                Thread.Sleep(wait);
            }
        }
    }

}
