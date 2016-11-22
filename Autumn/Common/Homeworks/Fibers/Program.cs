using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessManager.CurrentProcess = 0;
            int numOfFibers = 8;
            for (int i = 0; i < numOfFibers; i++)
            {
                Process proc = new Process();
                Fiber fiber = new Fiber(new Action(proc.Run));
                ProcessManager.FibersList.Add(fiber.Id);
                ProcessManager.Processes.Add(proc);
                ProcessManager.FibersToDelete.Add(fiber.Id);
            }
            ProcessManager.Switch(false);
            Console.ReadKey();
        }
    }
}
