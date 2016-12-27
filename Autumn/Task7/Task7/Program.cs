using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7
{
    class Program
    {
        static void Main(string[] args)
        {
            SlowSystem slowSystem = new SlowSystem();
            FastSystem fastSystem = new FastSystem();
            Emulation emul = new Emulation(slowSystem, 5);
            Console.WriteLine(emul.Result);
            emul = new Emulation(fastSystem, 5);
            Console.WriteLine(emul.Result);
            Console.ReadKey();
        }
    }
}
