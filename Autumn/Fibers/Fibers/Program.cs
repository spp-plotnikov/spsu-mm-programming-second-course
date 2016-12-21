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
            for (int i = 0; i < 5; i++)
            {
                ProcessManager.AddFiber(new Process());
            }

            ProcessManager.IsPriorityEnabled = true;
            ProcessManager.Switch(false);
            ProcessManager.DeleteFibers();;
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
