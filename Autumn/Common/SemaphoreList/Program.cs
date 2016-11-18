using System;
using System.Threading;

namespace SemaphoreExample
{
    class Program
    {
        

        

        static void Main(string[] args)
        {
            ListWSemaphore<int> list = new ListWSemaphore<int>();
            int numOfProducers = Constants.numOfProducers, numOfConsumers = Constants.numOfProducers; // init number of producers and consumers
            Console.WriteLine("Press any key to stop it");
            Thread.Sleep(2000);
            Producers producers = new Producers();
            Consumers consumers = new Consumers();
            producers.StartProducers(numOfProducers, list, 0);
            consumers.StartConsumers(numOfConsumers, list, 0);
            Console.ReadKey();
            producers.StopWorking();
            consumers.StopWorking();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            
        }
    }    
}