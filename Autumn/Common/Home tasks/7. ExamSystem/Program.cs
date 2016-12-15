using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace ExamSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                Checker checker = new Checker();

                ExamSystemFirst sysFirst = new ExamSystemFirst();
             //   string timeFirst = checker.GetTime(sysFirst);
              //  Console.WriteLine("Runtime first system = " + timeFirst);

                ExamSystemSecond sysSecond = new ExamSystemSecond();
                string timeSecond = checker.GetTime(sysSecond);
                Console.WriteLine("Runtime second system = " + timeSecond);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
