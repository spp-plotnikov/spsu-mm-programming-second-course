using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace ProcessManager
{
    public static class ProcessManager
    {
        private static Dictionary<uint, int> fibers = new Dictionary<uint, int>(); //fiberId: fiberpriority
        private static Dictionary<int, uint> fibersIter = new Dictionary<int, uint>(); //number of iter: fiberId
        private static List<uint> fibersList = new List<uint>();
        private static Queue<int> fibersQueue = new Queue<int>();
        private static int currentFiber = 0;
        private static int iteration = 0;

        public static void DeleteAll()
        {
            foreach (uint fiber in fibers.Keys)
            {
                Fiber.Delete(fiber);
            }
        }

        //  there are no priorities

        //public static void Switch(bool fiberFinished)
        //{
        //    if (!(fibersQueue.Count() > 0)) //Fiber.PrimaryId is working now
        //    {
        //        DeleteAll();
        //    }
        //    else if (fiberFinished)
        //    {
        //        Console.WriteLine(string.Format("Fiber{0} has finished", fibersList[currentFiber]));
        //        fibersQueue.Dequeue();
        //        if (fibersQueue.Count() > 0)
        //        {
        //            currentFiber = fibersQueue.Peek();
        //            Fiber.Switch(fibersList[currentFiber]);
        //        }
        //        else
        //        {
        //            Console.WriteLine("The end");
        //            Fiber.Switch(Fiber.PrimaryId);
        //        }
        //    }
        //    else
        //    {
        //        int first = fibersQueue.Dequeue();
        //        fibersQueue.Enqueue(first);
        //        currentFiber = fibersQueue.Peek();
        //        Fiber.Switch(fibersList[currentFiber]);
        //    }
        //}

        public static void Switch(bool fiberFinished)
        {
            Thread.Sleep(3);
            iteration = (iteration + 1) % (fibers.Count * 3);
            if (!(fibersList.Count > 0)) //Fiber.PrimaryId is working now
            {
                DeleteAll();
            }
            else if (fiberFinished)
            {
                Console.WriteLine(string.Format("Fiber{0} has finished", fibersList[currentFiber]));
                fibersList.RemoveAt((int)currentFiber);
                if (fibersList.Count() > 0)
                {
                    currentFiber = fibersList.Count - 1;
                    Fiber.Switch(fibersList[currentFiber]);
                }
                else
                {
                    Console.WriteLine("The end");
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                //to allow one process (maybe with lower priority) to get CPU time to work
                uint iter;
                if ((fibersIter.TryGetValue(iteration, out iter)) && (fibersList.Contains(iter)))
                {
                    currentFiber = fibersList.IndexOf(iter);
                    Fiber.Switch(iter);
                }
                else
                {
                    currentFiber = fibersList.Count() - 1;
                    Fiber.Switch(fibersList[currentFiber]);
                }
            }
        }

        public static void Main()
        {
            int fibers_num = 7;
            for (int i = 0; i < fibers_num; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                fibers.Add(fiber.Id, process.Priority);
                fibersIter.Add((i + 1) * 2, fiber.Id);
                fibersQueue.Enqueue(i);
                Console.WriteLine("{0}-{1}", fiber.Id, process.Priority);
            }
            fibers = fibers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            currentFiber = fibers.Count() - 1;
            fibersList = fibers.Keys.ToList();
            Switch(false);
            Console.ReadLine();
        }
    }

    public class Process
    {
        private static readonly Random Rng = new Random();

        private const int LongPauseBoundary = 10000;

        private const int ShortPauseBoundary = 100;

        private const int WorkBoundary = 1000;

        private const int IntervalsAmountBoundary = 10;

        private const int PriorityLevelsNumber = 10;

        private readonly List<int> _workIntervals = new List<int>();
        private readonly List<int> _pauseIntervals = new List<int>();

        public Process()
        {
            int amount = Rng.Next(IntervalsAmountBoundary);

            for (int i = 0; i < amount; i++)
            {
                _workIntervals.Add(Rng.Next(WorkBoundary));
                _pauseIntervals.Add(Rng.Next(
                        Rng.NextDouble() > 0.9
                            ? LongPauseBoundary
                            : ShortPauseBoundary));
            }

            Priority = Rng.Next(PriorityLevelsNumber);
        }

        public void Run()
        {
            for (int i = 0; i < _workIntervals.Count; i++)
            {
                Thread.Sleep(_workIntervals[i]); // work emulation
                DateTime pauseBeginTime = DateTime.Now;
                do
                {
                    ProcessManager.Switch(false);
                } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
            }
            ProcessManager.Switch(true);
        }

        public int Priority
        {
            get; private set;
        }

        public int TotalDuration
        {
            get
            {
                return ActiveDuration + _pauseIntervals.Sum();
            }
        }

        public int ActiveDuration
        {
            get
            {
                return _workIntervals.Sum();
            }
        }
    }
}
