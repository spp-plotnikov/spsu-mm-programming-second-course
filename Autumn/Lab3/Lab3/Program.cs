using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Lab3
{
    public class Program
    {
        static ConcurrentQueue<int> _buffer = new ConcurrentQueue<int>();
        static List<Producer> _listOfProducers = new List<Producer>();
        static List<Consumer> _listOfConsumers = new List<Consumer>();
        static Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            var rnd = new Random();
            int numberOfProducers = rnd.Next(1, 20);
            int numberOfConsumers = rnd.Next(20);
            Console.WriteLine("Number of produsers: " + numberOfProducers);
            Console.WriteLine("Number of consumers: " + numberOfConsumers);
            Console.WriteLine("Press any button to start/finish");
            Console.ReadKey();

            for (int i = 0; i < numberOfProducers; i++)
            {
                _listOfProducers.Add(new Producer(i, _buffer, _mutex));
            }

            for (int i = 0; i < numberOfConsumers; i++)
            {
                _listOfConsumers.Add(new Consumer(i, _buffer, _mutex));
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

            Thread.Sleep(100);
            Console.WriteLine("The end");
            Console.Read();
        }
    }
}