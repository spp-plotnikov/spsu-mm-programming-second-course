using Fibers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 5;
            for (int i = 0; i < number; i++)
            {
                Process process = new Process(i, i + 1);
                Fiber fiber = new Fiber(new Action(process.Run));
                ProcessManagerFramework.AddFiber(fiber, process);
            }
            ProcessManagerFramework.Switch(false);
            Console.ReadLine();
        }
    }
}
