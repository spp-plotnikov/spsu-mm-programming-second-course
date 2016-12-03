using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Deanery
{
    class TestingSystem
    {
        private KeyValuePair<long, long>[] containsRequests = new KeyValuePair<long, long>[900];
        private KeyValuePair<long, long>[] addRequests = new KeyValuePair<long, long>[90];
        private KeyValuePair<long, long>[] removeRequests = new KeyValuePair<long, long>[10];
        private IExamSystem system1;
        private IExamSystem system2;
        public TestingSystem(IExamSystem system1, IExamSystem system2)
        {
            this.system1 = system1;
            this.system2 = system2;
            Random rnd = new Random();
            for (int i = 0; i < 900; i++)
            {
                containsRequests[i] = new KeyValuePair<long, long>(rnd.Next(1, 100), rnd.Next(1, 10));
            }

            for (int i = 0; i < 90; i++)
            {
                addRequests[i] = new KeyValuePair<long, long>(rnd.Next(1, 100), rnd.Next(1, 10));
            }

            for (int i = 0; i < 10; i++)
            {
                removeRequests[i] = addRequests[i * 2];
            }
        }

        private void ContainsThread(int minIndex, int maxIndex, IExamSystem system)
        {
            for (int i = minIndex; i < maxIndex; i++)
            {
                system.Contains(containsRequests[i].Key, containsRequests[i].Value);
            }
        }

        private void AddThread(int minIndex, int maxIndex, IExamSystem system)
        {
            for (int i = minIndex; i < maxIndex; i++)
            {
                system.Add(addRequests[i].Key, addRequests[i].Value);
            }
        }

        private void RemoveThread(int minIndex, int maxIndex, IExamSystem system)
        {
            for (int i = minIndex; i < maxIndex; i++)
            {
                system.Remove(removeRequests[i].Key, removeRequests[i].Value);
            }
        }
        // Testing of SimpleImplementation
        public void StartTestOfSimple()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int numOfFinishedThreads = 0;
            for (int i = 0; i < 2; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    AddThread(temp * 45, temp * 45 + 45, system1); // Here's the problem
                    numOfFinishedThreads++;
                });
                thread.Start();
            }

            for (int i = 0; i < 9; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    ContainsThread(temp * 90, temp * 90 + 90, system1);
                    numOfFinishedThreads++;
                });
                thread.Start();
            }

            for (int i = 0; i < 2; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    RemoveThread(temp * 5, temp * 5 + 5, system1);
                    numOfFinishedThreads++;
                });
                thread.Start();
            }
            while(numOfFinishedThreads < 13) { Thread.Sleep(100); }
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime of SimpleImplementation " + elapsedTime);
        }
        // Testing of NotTrivialImplementation
        public void StartTestOfNotTrivial()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int numOfFinishedThreads = 0;
            for (int i = 0; i < 2; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    AddThread(temp * 45, temp * 45 + 45, system2);
                    numOfFinishedThreads++;
                });
                thread.Start();
            }

            for (int i = 0; i < 9; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    ContainsThread(temp * 90, temp * 90 + 90, system2);
                    numOfFinishedThreads++;
                });
                thread.Start();
            }

            for (int i = 0; i < 2; i++)
            {
                int temp = i;
                Thread thread = new Thread(() =>
                {
                    RemoveThread(temp * 5, temp * 5 + 5, system2);
                    numOfFinishedThreads++;
                });
                thread.Start();
            }
            while (numOfFinishedThreads < 13) { Thread.Sleep(100); }
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime of NotTrivialImplementation " + elapsedTime);
        }
    }
}
