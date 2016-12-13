using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab_3_PandC
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NumOfC = 5;
            const int NumOfP = 5;
            Queue<int> buf = new Queue<int>();

            Producer producers = new Producer(NumOfP, buf);
            Consumer consumers = new Consumer(NumOfC, buf);

            Console.ReadLine();
            producers.Stop();
            consumers.Stop();
        }
    }
}