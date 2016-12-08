using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ExamStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmark bench = new Benchmark(new SlowStorage());
            //Benchmark bench = new Benchmark(new FastStorage());

            bench.StartTesting();
            Console.ReadKey();
        }
    }
}
