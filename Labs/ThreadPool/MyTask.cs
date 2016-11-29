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
            int r = new Random(num).Next(1000, 5000) * (num + 3) / (num + 1);
            Console.WriteLine("{0} start, work {1} on {2}", name, r, Thread.CurrentThread.Name);
            Thread.Sleep(r);
        }
    }
}
