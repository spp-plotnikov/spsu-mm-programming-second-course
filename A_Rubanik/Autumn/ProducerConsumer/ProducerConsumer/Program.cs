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
        static int NumberOfProducer = 5;
        static int NumberOfConsumer = 5;
        public static List<int> list;
        public static Mutex mutex = new Mutex();
        static void Main(string[] args)
        {
            Producer[] producers = new Producer[NumberOfProducer];
            Consumer[] consumers = new Consumer[NumberOfConsumer];
            int amountOfThreads = NumberOfConsumer + NumberOfProducer;
            Thread[] threads = new Thread[amountOfThreads];
            list = new List<int>();
            for(int i = 0; i < NumberOfProducer; i++)
            {
                producers[i] = new Producer(i);
            }
            for(int i = 0; i < NumberOfConsumer; i++)
            {
                consumers[i] = new Consumer(i);
            }
            for(int i =0; i< NumberOfProducer; i++)
            {
                threads[i] = new Thread(producers[i].Producing);
                threads[i].Start();
            }
            for (int i = NumberOfProducer; i < amountOfThreads; i++)
            {
                threads[i] = new Thread(consumers[i - NumberOfProducer].Consumering);
                threads[i].Start();
            }
            
            for(int i = 0; i < amountOfThreads; i++)
            {
                threads[i].Join();
            }
        }
        
    }
}
