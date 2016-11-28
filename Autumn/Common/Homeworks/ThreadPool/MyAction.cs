using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public static class MyAction
    {
        public static void DoTheJob()
        {
            Console.WriteLine("Hello, I'm the new job, which will be finished soon.");
            Console.WriteLine("The task is completed");
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");
        }
    }
}
