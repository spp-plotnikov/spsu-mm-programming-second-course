using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    class Program
    {
        static void Main(string[] args)
        {
            LockSystem lockSystem = new LockSystem();
            Timer countLockSystem = new Timer(lockSystem, "LockSystem");
            countLockSystem.CountTime();
            ComplicatedSystem complicatedSystem = new ComplicatedSystem();
            Console.WriteLine("Press any button to test the next system!");
            Console.ReadKey();
            Timer countComplicatedSystem = new Timer(complicatedSystem, "ComplicatedSystem");
            countComplicatedSystem.CountTime();
            Console.ReadKey();
        }
    }
}
