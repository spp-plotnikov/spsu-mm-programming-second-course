using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool pool = new ThreadPool();
            pool.Start();
            Tasks tasks = new Tasks(pool);
            tasks.Start();

            Console.ReadKey();
            Console.WriteLine("\nStoping");
            tasks.Stop();
            pool.Dispose();
            Console.WriteLine("Stoped");
            Console.ReadLine();
        }
    }
}
