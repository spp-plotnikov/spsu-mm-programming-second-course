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
            Checker checker = new Checker();
            Console.WriteLine("Locks\tMin t\t\tMax t\t\tAvg t\t\tMedian t");
            for (int num=5;num<=25;num++)
            {
                List<long> ticks = new List<long>();
                for(int i=0;i<10000;i++)
                {
                    ExamSystemSecond sysSecond = new ExamSystemSecond(num);
                    ticks.Add(checker.GetTime(sysSecond));
                }
                ticks.Sort();

                Console.WriteLine("{0}\t{1:s\\:ffffff}\t{2:s\\:ffffff}\t{3:s\\:ffffff}\t{4:s\\:ffffff}", 
                    num, 
                    new TimeSpan(ticks.Min()),
                    new TimeSpan(ticks.Max()),
                    new TimeSpan((long)ticks.Average()),
                    new TimeSpan(ticks[ticks.Count/2]));

            }
            
            Console.ReadLine();
        }
    }
}
