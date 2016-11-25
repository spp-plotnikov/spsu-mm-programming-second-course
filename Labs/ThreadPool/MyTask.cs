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
        int num = 1;

        public MyTask(int n)
        {
            name = "Task " + n;
            num = n;
        }
           
        public void StartWork()
        {
            int r = new Random().Next(1000, 10000) * (num + 1) / 3;
            Console.WriteLine("{0} start, work {1}", name, r);
            Thread.Sleep(r);
        }
    }
}
