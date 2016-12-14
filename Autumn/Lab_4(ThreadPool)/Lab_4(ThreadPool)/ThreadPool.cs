using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab_4_ThreadPool
{
    public class ThreadPool : IDisposable
    {
        public const int NumOfThreads = 4;
        bool isPoolAlive;
        Queue<Action> actionQueue;
        Thread[] threads = new Thread[NumOfThreads];

        public ThreadPool()
        {
            isPoolAlive = true;
            actionQueue = new Queue<Action>();

            for(int curThread = 0; curThread < NumOfThreads; curThread++)
            {
                threads[curThread] = new Thread(ThreadAct);
                threads[curThread].Name = curThread.ToString();
            }
        }

        public void Start()
        {
            foreach(var thread in threads)
            {
                thread.Start();
            }
        }

        void ThreadAct()
        {
            while(isPoolAlive)
            {
                Monitor.Enter(actionQueue);
                if(actionQueue.Count != 0)
                {
                    Action act = actionQueue.Dequeue();
                    Monitor.Exit(actionQueue);
                    act();
                }
                else
                {
                    Console.WriteLine("Im waiting  {0}", Thread.CurrentThread.Name);
                    Monitor.Wait(actionQueue);
                    Console.WriteLine("I get pulse {0}", Thread.CurrentThread.Name);
                    Monitor.Exit(actionQueue);
                }
            }
            Console.WriteLine("Im dead now {0}",Thread.CurrentThread.Name);
        }

        public void Enqueue(Action a)
        {
            Monitor.Enter(actionQueue);
            actionQueue.Enqueue(a);
            Monitor.Pulse(actionQueue);
            Monitor.Exit(actionQueue);
        }

        public void Dispose()
        {
            Console.WriteLine("Kill them all now!");
            isPoolAlive = false;

            Monitor.Enter(actionQueue);
            Monitor.PulseAll(actionQueue);
            Monitor.Exit(actionQueue);

            foreach(var thread in threads)
            {
                thread.Join();
            }
            Console.ReadKey();
        }
    }
}