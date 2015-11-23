using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class MyThreadPool : IDisposable
    {
        delegate void FunctionOfEndWork();
        event FunctionOfEndWork FinishingWork;
        int NumberOfThreads;
        List<MyThread> Threads;

        public MyThreadPool(int n)
        {
            NumberOfThreads = n;
            Threads = new List<MyThread>();
            for (int i = 0; i < n; i++)
            {
                Threads.Add(new MyThread(i));
                FinishingWork += Threads[i].EndWork;
            }
        }

        public void Add(Action a)
        {
            int NumberOfMethodsInFirstThread = Threads[0].QueueOfMethods.Count;
            int MinIndex = 0;
            foreach (MyThread n in Threads)
            {
                n.LockerOfQueue.WaitOne();
                int NumberOfMethodsInCurrentThread = n.QueueOfMethods.Count;
                n.LockerOfQueue.ReleaseMutex();
                if (NumberOfMethodsInCurrentThread < NumberOfMethodsInFirstThread)
                {
                    NumberOfMethodsInFirstThread = n.QueueOfMethods.Count;
                    MinIndex = Threads.IndexOf(n);
                }
            }
            Threads[MinIndex].LockerOfQueue.WaitOne();
            Threads[MinIndex].Enqueue(a);
            Threads[MinIndex].LockerOfQueue.ReleaseMutex();
        }
        public void Dispose()
        {
            FinishingWork();
        }
    }
}
