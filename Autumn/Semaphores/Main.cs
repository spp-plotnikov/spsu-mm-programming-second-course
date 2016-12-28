using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphores
{
    class Program
    {
        static void Main(string[] args)
        {
            SemManager MySemManager = new SemManager();
            MySemManager.StartP(10);
            MySemManager.StartC(4);

            Console.ReadLine();

            MySemManager.StopP();
            Console.ReadLine();
            MySemManager.StopC();
        }
    }
}
