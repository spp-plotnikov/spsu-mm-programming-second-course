using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var cnv = new Conveyor<string>();
            List<Producer<string>> producers = new List<Producer<string>>();
            List<Consumer<string>> consumers = new List<Consumer<string>>();

            for (int i = 0; i < 3; i++)
            {
                producers.Add(new Producer<string>(200, new Random().Next(), cnv));
            }

            Console.WriteLine("FACTORY IS ON!!! Press any key to stop it");

            for (int i = 0; i < 10; i++)
            {
                consumers.Add(new Consumer<string>(500, cnv));
            }

            foreach (var producer in producers)
            {
                Thread thread = new Thread(() =>
                {
                    producer.StartProducing();
                });
                thread.Start();
            }

            foreach (var consumer in consumers)
            {
                Thread thread = new Thread(() =>
                {
                    consumer.StartConsuming();
                });
                thread.Start();
            }


            Console.ReadKey();

            foreach (var producer in producers)
            {
                producer.StopProducing();
            }

            foreach (var consumer in consumers)
            {
                consumer.StopConsuming();
            }
        }
    }
}
