using System;
using System.Threading;

namespace SemaphoreExample
{
    class Program
    {
        static void StartProducers(int numOfProducers, ThreadStart startPoint) // Run producers and init its names 
        {
            for (int i = 0; i < numOfProducers; i++)
            {
                Thread producer = new Thread(startPoint);
                producer.Name = "Producer #" + i.ToString();
                producer.Start();
            }
        }

        static void StartConsumers(int numOfConsumers, ThreadStart startPoint) // // Run consumers and init its names
        {
            for (int i = 0; i < numOfConsumers; i++)
            {
                Thread consumer = new Thread(startPoint);
                consumer.Name = "Consumer #" + i.ToString();
                consumer.Start();
            }
        }

        static void Main(string[] args)
        {
            ListWSemaphore<int> list = new ListWSemaphore<int>();
            int numOfProducers = Constants.numOfProducers, numOfConsumers = Constants.numOfProducers; // init number of producers and consumers
            Console.WriteLine("Press any key to stop it");
            Thread.Sleep(2000);
            StartProducers(numOfProducers, new ThreadStart(() => Producer.StartCycle(list, 0)));
            StartConsumers(numOfConsumers, new ThreadStart(() => Consumer.StartCycle(list, 0)));
            Console.ReadKey();
            list.LockFrvr();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            
        }
    }    
}