using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManager;

namespace Fibers_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            MyProcessManager.Emulation(4, true);
        }
    }
}
