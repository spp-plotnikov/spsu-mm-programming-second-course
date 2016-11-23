using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        public static bool Working = true;

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

            List <Producer> prods = new List <Producer>();
            List <Consumer> cons = new List <Consumer>();

            Console.WriteLine("Press the key to begin");
            Console.WriteLine("Press the key again to stop the process");
            Console.ReadKey();

            // creating threads for producers
            for (int i = 0; i < numOfProducers; i++)
            {
                Producer prod = new Producer(i, goods);
                prods.Add(prod);
            }

            // creating threads for consumers
            for (int i = 0; i < numOfConsumers; i++)
            {
                Consumer con = new Consumer(i, goods);
                cons.Add(con);
            }

            Console.ReadKey(true);
            Working = false;

            for (int i = 0; i < numOfProducers; i++)
            {
                prods[0].Finish();
                prods.RemoveAt(0);
            }
            
            for (int i = 0; i < numOfConsumers; i++)
            {
                cons[0].Finish();
                cons.RemoveAt(0);
            }
            
            Console.WriteLine("All threads finished");
            Console.ReadKey();
        }
    }
}
