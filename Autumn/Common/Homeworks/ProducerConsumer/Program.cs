using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool res = false;
            int numOfProducers = 0;
            int numOfConsumers = 0;
            List<int> goods = new List<int>();

            // reading the initial number of producers and consumers
            Console.WriteLine("Enter the number of producers:");
            while (!res)
                res = Int32.TryParse(Console.ReadLine(), out numOfProducers);

            res = false;
            Console.WriteLine("Enter the number of consumers:");
            while (!res)
                res = Int32.TryParse(Console.ReadLine(), out numOfConsumers);

            Producer[] prods = new Producer[numOfProducers];
            Consumer[] cons = new Consumer[numOfConsumers];

            Console.WriteLine("Press the key to begin");
            Console.WriteLine("Press the key again to stop the process");
            Console.ReadKey();

            // creating threads for producers
            for (int i = 0; i < numOfProducers; i++)
            {
                prods[i] = new Producer(i, goods);
            }

            // creating threads for consumers
            for (int i = 0; i < numOfConsumers; i++)
            {
                cons[i] = new Consumer(i, goods);
            }

            Console.ReadKey(true);
            Console.WriteLine("All threads will be finished soon");

            for (int i = 0; i < numOfProducers; i++)
            {
                prods[i].State = false;
            }

            for (int i = 0; i < numOfConsumers; i++)
            {
                cons[i].State = false;
            }

            for (int i = 0; i < numOfConsumers; i++)
            {
                cons[i].Finish();
            }
            
            for (int i = 0; i < numOfProducers; i++)
            {
                prods[i].Finish();
            }
            
            Console.WriteLine("All threads finished");
            Console.ReadKey();
        }
    }
}
