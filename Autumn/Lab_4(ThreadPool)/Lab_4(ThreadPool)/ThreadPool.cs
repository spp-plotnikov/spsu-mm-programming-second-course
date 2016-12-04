using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab_4_ThreadPool
{
    public class ThreadPool : IDisposable
    {
        public const int NumOfThreads = 6;
        bool poolAlive;
        bool exitIfNoWork;
        Queue<Action> actionQueue;
        Thread[] threads = new Thread[NumOfThreads];

        public ThreadPool()
        {
            poolAlive = true;
            exitIfNoWork = true;
            actionQueue = new Queue<Action>();

            for (int curThread = 0; curThread < NumOfThreads; curThread++)
                threads[curThread] = new Thread(threadAct);
        }

        void threadAct()
        {
            Action act = null;
            while (poolAlive)
            {
                if (actionQueue.Count != 0)
                {
                    Monitor.Enter(actionQueue);
                    act = actionQueue.Dequeue();
                    Monitor.Exit(actionQueue);
                    act();
                }
                else if (exitIfNoWork)
                {
                    poolAlive = false;
                    Console.WriteLine("Queue is empty.");
                    Console.WriteLine("completion of the current threads");

                    foreach (Thread thread in threads)
                        if (Thread.CurrentThread != thread)
                        {
                            thread.Join();
                        }

                    Console.WriteLine("Work is completed.");
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
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
        }

        public void Dispose()
        {
            if (poolAlive)
            {
                poolAlive = false;
                Console.WriteLine("Interrupted by user");
                Console.WriteLine("completion of the current threads");

                foreach (Thread thread in threads)
                    thread.Join();
                Console.WriteLine("Work ended prematurely.");
                Console.ReadKey();
            }
        }
    }
}