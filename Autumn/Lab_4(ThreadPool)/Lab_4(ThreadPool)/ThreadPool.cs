using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab_4_ThreadPool
{
    public class ThreadPool : IDisposable
    {
        public const int NumOfThreads = 6;
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
            }
        }

        void ThreadAct()
        {
            Action act = null;
            while(isPoolAlive)
            {
                if(actionQueue.Count != 0)
                {
                    Monitor.Enter(actionQueue);
                    act = actionQueue.Dequeue();
                    Monitor.Exit(actionQueue);
                    act();
                }
                else
                {
                    Thread.Sleep(100); // pause between checks
                }
            }
        }

        public void Enqueue(Action a)
        {
            Monitor.Enter(actionQueue);
            actionQueue.Enqueue(a);
            Monitor.Exit(actionQueue);
        }

        public void Start()
        {
            foreach(Thread thread in threads)
            {
                thread.Start();
            }
        }

        public void Dispose()
        {
            if(isPoolAlive)
            {
                isPoolAlive = false;
                Console.WriteLine("Interrupted by user");
                Console.WriteLine("completion of the current threads");

                foreach(Thread thread in threads)
                {
                    thread.Join();
                }
                Console.WriteLine("Work ended.");
                Console.ReadKey();
            }
        }
    }
}