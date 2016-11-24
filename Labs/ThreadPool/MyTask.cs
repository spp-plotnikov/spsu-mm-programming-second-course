using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class MyTask
    {
        string name;

        public MyTask(int n)
        {
            name = "Task " + n;
        }
           
        public void StartWork()
        {
            int r = new Random().Next(1000, 10000);
            Console.WriteLine("{0} start, work {1}", name, r);
            Thread.Sleep(r);
        }
    }
}
