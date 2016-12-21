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

            ProcessManager.Priority = priority;
            for (int i = 0; i < numberOfFibers; i++)
            {
                Process proc = new Process();
                ProcessManager.Processes.Add(proc);
                Fiber fiber = new Fiber(new Action(proc.Run));
                ProcessManager.Fibers.Add(fiber.Id);
                ProcessManager.CurFiber = i == 0 ? fiber.Id : ProcessManager.CurFiber;
            }

            Console.WriteLine("Number of fibers: {0}, priority: {1}", numberOfFibers, priority ? "yes" : "no");
            Console.WriteLine("Press any button to start");
            Console.ReadKey();

            ProcessManager.Switch(false);
            Console.Read();
        }
    }
}
