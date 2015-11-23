using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Writing()
        {
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine("It's " + i);
            }
        }

        static void Main(string[] args)
        {
            MyThreadPool Pool = new MyThreadPool(7);
            for (int i = 1; i < 3; i++)
            {
                Pool.Add(Writing);
            }
            Console.ReadLine();
            Pool.Dispose();
        }


    }
}
