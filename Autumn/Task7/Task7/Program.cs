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
            int numberOfTests = 35;
            Emulation emul = new Emulation(slowSystem, 5);
            Test(emul, numberOfTests);
            emul = new Emulation(fastSystem, 5);
            Test(emul, numberOfTests);
            Console.ReadKey();
        }

        public static void Test(Emulation emul, int numberOfTests)
        {
            double average = 0.0;
            emul.Init();
            for(int i = 0; i < 10; i++)
            {
                emul.Start();
            }
            for (int i = 0; i < numberOfTests; i++)
            {
                emul.Start();
                average += emul.Result;
            }
            Console.WriteLine(average / numberOfTests);
        }
    }
}
