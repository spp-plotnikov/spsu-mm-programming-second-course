using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Producer_consumer
{
    class Program
    {

        private static int NumConsumers = 3;
        private static int NumProducers = 3;

        static void Main(string[] args)
        {
            Additional main = new Additional(NumConsumers, NumProducers);
            main.Add();

            Console.ReadKey();
            Console.WriteLine("\nStopping all threads...");

            main.Close();

            Console.WriteLine("All process was stopped");
            Console.ReadKey();
        }
    }
}
