using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            // reading the initial number of producers and consumers

            Console.WriteLine("Enter the number of producers:");
            while (!res)
                res = Int32.TryParse(Console.ReadLine(), out numOfProducers);

            for (int i = 0; i < numOfProducers; i++)
            {
                Producer prod = new Producer(i + 1);
                Workspace.Producers.Add(prod);
            }
            
            res = false;
            Console.WriteLine("Enter the number of consumers:");
            while (!res)
                res = Int32.TryParse(Console.ReadLine(), out numOfConsumers);

            for (int i = 0; i < numOfConsumers; i++)
            {
                Consumer cons = new Consumer(i + 1);
                Workspace.Consumers.Add(cons);
            }

            Console.WriteLine("Press the key to begin");
            Console.WriteLine("Press the key again to stop the process");
            Console.ReadKey();

            Workspace.DoTheWork();
        }
    }
}
