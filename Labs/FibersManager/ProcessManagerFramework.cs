using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace FibersManager
{
    public static class ProcessManager
    {
        static List<Fiber> fibers = new List<Fiber>();
        static Dictionary<uint, int> idPrior = new Dictionary<uint, int>(); //fiberId: fiberpriority
        static Queue<int> fibQueue = new Queue<int>();
        static int fibNow = 0;

        static void create(uint size, bool prior)
        {
            for (int i = 0; i < size; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                fibers.Add(fiber);
                idPrior.Add(fiber.Id, process.Priority);
                fibQueue.Enqueue(i);
                Console.WriteLine("{0}-{1}", fiber.Id, process.Priority);
            }
            fibNow = fibQueue.Count() - 1;
            if (prior)
            {
                idPrior = idPrior.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                List<int> list = idPrior.Values.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    Fiber fib = fibers[0];

                    foreach (var f in fibers)
                    {
                        if (f.Id == list[i])
                        {
                            fib = f;
                            break;
                        }
                    }
                    fibers.Remove(fib);
                    fibers.Add(fib);
                }
            }
        }

        static void DeleteAll()
        {
            foreach (uint fib in idPrior.Keys)
            {
                Fiber.Delete(fib);
            }
        }

        ////not priority
        //public static void Switch(bool fiberFinished)
        //{
        //    if (fibers.Count() <= 0) return;
        //    if (fiberFinished)
        //    {
        //        Console.WriteLine(string.Format("{0} has finished", fibers[fibNow].Id));
        //        fibQueue.Dequeue();
        //        if (fibQueue.Count() > 0)
        //        {
        //            fibNow = fibQueue.Peek();
        //            Fiber.Switch(fibers[fibNow].Id);
        //        }
        //        else
        //        {
        //            Console.WriteLine("The end");
        //            Fiber.Switch(Fiber.PrimaryId);
        //        }
        //    }
        //    else
        //    {
        //        int first = fibQueue.Dequeue();
        //        fibQueue.Enqueue(first);
        //        fibNow = fibQueue.Peek();
        //        Fiber.Switch(fibers[fibNow].Id);
        //    }
        //}

        //priority
        public static void Switch(bool fiberFinished)
        {
            if (fibers.Count <= 0) return;
            if (fiberFinished)
            {
                Console.WriteLine(string.Format("Fiber{0} has finished", fibers[fibNow].Id));
                fibers.RemoveAt(fibNow);
                if (fibers.Count() > 0)
                {
                    fibNow = fibers.Count() - 1;
                    Fiber.Switch(fibers[fibNow].Id);
                }
                else
                {
                    Console.WriteLine("The end");
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                fibNow = fibers.Count() - 1;
                Fiber.Switch(fibers[fibNow].Id);
            }
        }


        static void Main()
        {
            bool prior = true;
            Console.WriteLine("Input number of fibers:");
            uint size = Convert.ToUInt32(Console.ReadLine());
            create(size, prior);
            
            Switch(false);
            Console.ReadKey();
            DeleteAll();
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
