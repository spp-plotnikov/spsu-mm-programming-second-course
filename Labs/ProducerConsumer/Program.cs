using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class ProdCons
    {
        Semaphore _mut, _empty, _full;
        List<int> items = new List<int>();

        public ProdCons(int num) //емкость буфера
        {
            _mut = new Semaphore(1, num);
            _empty = new Semaphore(num, num); 
            _full = new Semaphore(0, num);      
        }

        public void Producer()
        {
            Random r = new Random();
            while (true)
            {
                //Console.WriteLine("{0} is waiting in QUEUE...", Thread.CurrentThread.Name);
                _empty.WaitOne();
                _mut.WaitOne();
               // Console.WriteLine("{0} enters the Critical Section!", Thread.CurrentThread.Name);
                items.Add(r.Next(100));
                Console.WriteLine(items.Count);
              //  Console.WriteLine("{0} is leaving the Critical Section", Thread.CurrentThread.Name);
                _mut.Release();
                _full.Release();
                Thread.Sleep(1000);
            }
        }

        public void Consumer()
        {
            while (true)
            {
                //Console.WriteLine("{0} is waiting in QUEUE...", Thread.CurrentThread.Name);
                _full.WaitOne();
                _mut.WaitOne();
                //Console.WriteLine("{0} enters the Critical Section!", Thread.CurrentThread.Name);
                items.RemoveAt(items.Count - 1);
                Console.WriteLine(items.Count);
               // Console.WriteLine("{0} is leaving the Critical Section", Thread.CurrentThread.Name);
                _mut.Release();
                _empty.Release();
                Thread.Sleep(1000);
            }

        }

    }


    class Program
    {

        static void Main(string[] args)
        {
            int numbOfProd = 0, numbOfCons = 0;
            Console.WriteLine("Input numb of Producer: ");
            numbOfProd = Convert.ToInt32(Console.ReadLine());

            while (numbOfProd < 0)
            {
                Console.WriteLine("Not natural number. Input numb of Producers again: ");
                numbOfProd = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Input numb of Consumer: ");
            numbOfCons = Convert.ToInt32(Console.ReadLine());
            while (numbOfCons < 0)
            {
                Console.WriteLine("Not natural number. Input numb of Consumers again: ");
                numbOfCons = Convert.ToInt32(Console.ReadLine());
            }

            int total = 10000;
            Thread[] thr = new Thread[numbOfCons + numbOfProd];
            int j = 0;

            for (int i = 0; i < Math.Max(numbOfProd, numbOfCons); i++)
            {
                if (i < numbOfProd)
                {
                    Thread thread = new Thread(new ProdCons(total).Producer);
                    thr[j] = thread;
                    j++;
                    thread.Name = "Producer_" + i;
                    thread.Start();
                }

                if (i < numbOfCons)
                {
                    Thread thread = new Thread(new ProdCons(total).Consumer);
                    thr[j] = thread;
                    j++;
                    thread.Name = "Consumer_" + i;
                    thread.Start();
                }
            }

           while(!Console.KeyAvailable)
           {

           }

           foreach(var t in thr)
            {
                t.Abort();
                t.Join();
            }          
        }
    }
}
