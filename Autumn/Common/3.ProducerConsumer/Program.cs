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
        const int prodNum = 5;
        const int consNum = 5;

        static void Main(string[] args)
        {
            List<int> sharedData = new List<int>();
            List<Producer> prodList = new List<Producer>();
            List<Consumer> consList = new List<Consumer>();

            // starting producers
            for (int i = 0; i < prodNum; ++i)
            {
                Producer pr = new Producer(sharedData);
                prodList.Add(pr);
            }

            // starting consumers
            for (int i = 0; i < consNum; ++i)
            {
                Consumer co = new Consumer(sharedData);
                consList.Add(co);
            }

            // "kill" signal
            Console.ReadKey();
            Console.WriteLine("Stopping all threads...");

            // stopping and clearing list
            for (int i = 0; i < prodNum; ++i)
            {
                prodList[0].Stop();
                prodList.RemoveAt(0);
            }

            for (int i = 0; i < consNum; ++i)
            {
                consList[0].Stop();
                consList.RemoveAt(0);
            }
     
            // final state of sharedDate
            sharedData.ForEach(i => Console.Write(i + " "));
            Console.ReadKey();
        }
    }
}
