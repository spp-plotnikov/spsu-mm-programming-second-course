﻿using System;
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
            List<Producer> currentProducer = new List<Producer>();
            List<Consumer> currentConsumer = new List<Consumer>();
            Flag isLocked = new Flag();
            

            const int numOfProd = 3;
            const int numOfCons = 3;


            List<int> sharedList = new List<int>();

            for (int i = 0; i < numOfProd; i++)
            {
                Producer prod = new Producer(i + 1, sharedList, isLocked);
                currentProducer.Add(prod);
            }

            for (int i = 0; i < numOfCons; i++)
            {
                Consumer cons = new Consumer(i + 1, sharedList, isLocked);
                currentConsumer.Add(cons);
            }
            Console.ReadKey();

            Console.WriteLine("Wait little bit");

            

            for (int i = 0; i < numOfCons; i++)
            {
                currentConsumer[0].SetIsEnd();
                currentConsumer[0].Stop();
                currentConsumer.RemoveAt(0);
            }
            for (int i = 0; i < numOfProd; i++)
            {
                currentProducer[0].SetIsEnd();
                currentProducer[0].Stop();
                currentProducer.RemoveAt(0);
            }

            Console.WriteLine("that's all");

            return;
        }     
    }

    public class Flag
    {
        int isLocked = 0;
        
        public void Lock()
        {
            while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0))
            { }
        }

        public void UnLock()
        {
            Interlocked.Exchange(ref isLocked, 0);
        }
    }

    public class Producer
    {
        int name;
        Thread myThread;
        List<int> sharedList;
        const int wait = 1000;
        bool isEnd = false;
        bool endOfRun = false;

        public Producer(int name, List<int> sharedList, Flag isLocked)
        {
            this.name = name;
            this.sharedList = sharedList;
            myThread = new Thread(() => this.Run(isLocked));
            myThread.Start();
        }


        public void SetIsEnd()
        {
            this.isEnd = true;
        }

        public void Stop()
        {
            while (!endOfRun) { }
            myThread.Join();
        }

        public void Run(Flag isLocked)
        {
            while (!this.isEnd)
            {
                isLocked.Lock();
                if (!this.isEnd)
                {
                    Random rand = new Random();
                    int val = rand.Next(100);
                    sharedList.Add(val);
                    Console.WriteLine("Producer # {0} add number {1} to list", name, val);
                }
                isLocked.UnLock();
                Thread.Sleep(wait);
            }
            endOfRun = true;
        }

    }

    public class Consumer
    {
        int name;
        Thread myThread;
        List<int> sharedList;
        int wait = 1000;
        bool isEnd = false;
        bool endOfRun = false;

        public Consumer(int name, List <int> sharedList, Flag isLocked)
        {
            this.name = name;
            this.sharedList = sharedList;
            myThread = new Thread(() => this.Run(isLocked));
            myThread.Start();
        }

        public void SetIsEnd()
        {
            this.isEnd = true;
        }
        
        public void Stop()
        {
            while (!endOfRun) { }
            myThread.Join();
        }

        public void Run(Flag isLocked)
        {
            while (!this.isEnd)
            {
                isLocked.Lock();
                if (!this.isEnd)
                {
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
                }
                isLocked.UnLock();
                Thread.Sleep(wait);
            }
            endOfRun = true;
        }
    }

}
