using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab3
{
    public class Program
    {
        public static List<int> Buffer = new List<int>();
        static List<Producer> _listOfProducers = new List<Producer>();
        static List<Consumer> _listOfConsumers = new List<Consumer>();

        static void Main(string[] args)
        {
            var rnd = new Random();
            int numberOfProducers = rnd.Next(1, 10);
            int numberOfConsumers = rnd.Next(10);
            Console.WriteLine("Number of produsers: " + numberOfProducers);
            Console.WriteLine("Number of consumers: " + numberOfConsumers);
            Console.WriteLine("Press any button to start/finish");
            Console.ReadKey();

            for (int i = 0; i < numberOfProducers; i++)
            {
                _listOfProducers.Add(new Producer(i));
            }

            for (int i = 0; i < numberOfConsumers; i++)
            {
                _listOfConsumers.Add(new Consumer(i));
            }

            foreach (var prod in _listOfProducers)
            {
                Thread thread = new Thread(() => prod.Process(ref Buffer));
                thread.Start();
            }

            foreach (var cons in _listOfConsumers)
            {
                Thread thread = new Thread(delegate () { cons.Process(ref Buffer); });
                thread.Start();
            }

            Console.ReadKey();

            foreach (var prod in _listOfProducers)
            {
                prod.Stop();
            }

            foreach (var cons in _listOfConsumers)
            {
                cons.Stop();
            }

            Thread.Sleep(1000);
            Console.WriteLine("The end");
            Console.Read();
        }
    }
}
