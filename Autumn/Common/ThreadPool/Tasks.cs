using System;
using System.Threading;

namespace ThreadPool
{
    class Tasks
    {
        static public void Something1()
        {
            Console.WriteLine("Something1 Hello from Thread #{0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(100);
        }

        static public void Something2()
        {
            Console.WriteLine("Something2 Hello from Thread #{0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(100);
        }
    }
}
