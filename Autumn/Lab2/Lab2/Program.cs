using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            int numberOfFibers = rnd.Next(3, 20);
            bool priority = (rnd.Next(0, 2) == 1) ? true : false;
            Console.WriteLine("Number of fibers: {0}, priority: {1}", numberOfFibers, (priority ? "yes" : "no"));
            Console.WriteLine("Press any button to start");
            Console.ReadKey();

            ProcessManager.Do(numberOfFibers, priority);
            ProcessManager.Switch(false);
            Console.Read();
        }
    }
}
