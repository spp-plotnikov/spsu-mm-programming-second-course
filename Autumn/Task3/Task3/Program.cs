using System;
using System.Collections.Generic;

namespace Task3
{
    class Program
    {
        private const int producerSleep = 1000;
        private const int consumerSleep = 5000;

        static void Main(string[] args)
        {
            List<int> data = new List<int>();

            AtomicLock locker = new AtomicLock();

            List<Producer> prodList = new List<Producer>();
            List<Consumer> consList = new List<Consumer>();

            int producerNumber, consumerNumber;
            try
            {
                Console.WriteLine("Input number of producers, then number of consumers.");
                producerNumber = Convert.ToInt32(Console.ReadLine());
                consumerNumber = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Incorrect input, setting to default.");
                producerNumber = 5;
                consumerNumber = 5;
            }
            Console.WriteLine("Press enter to begin. Press enter again to finish simulation.");

            for(int i = 0; i < producerNumber; i++)
            {
                Producer producer = new Producer(locker, data, producerSleep);
                prodList.Add(producer);
            }
            
            for(int i = 0; i < consumerNumber; i++)
            {
                Consumer consumer = new Consumer(locker, data, consumerSleep);
                consList.Add(consumer);
            }
            
            Console.ReadLine();
            
            for(int i = 0; i < producerNumber; i++)
                prodList[i].Stop();

            for(int i = 0; i < consumerNumber; i++)
                consList[i].Stop();

            Console.WriteLine("Remains of the data:");
            foreach(var item in data)
            {
                Console.WriteLine(item + ", ");
            }
            Console.WriteLine("that's all.");
            Console.ReadKey();
        }
    }
}
