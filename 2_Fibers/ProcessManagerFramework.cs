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
        private static Dictionary<int, uint> fibers_iter = new Dictionary<int, uint>(); //number of iter: fiberId
        private static List<uint> fibers_list = new List<uint>();
        private static int current_fiber = 0;
        private static int iteration = 0;

        public static void Delete_All()
        {
            foreach (uint fiber in fibers.Keys)
            {
                Fiber.Delete(fiber);
            }
        }

        public static void Switch(bool fiberFinished)
        {
            Thread.Sleep(3);
            iteration = (iteration + 1) % (fibers.Count * 3);
            if (!(fibers_list.Count > 0)) //Fiber.PrimaryId is working now
            {
                Delete_All();
            }
            else if (fiberFinished)
            {
                Console.WriteLine(string.Format("Fiber{0} has finished", fibers_list[current_fiber]));
                fibers_list.RemoveAt((int)current_fiber);
                if (fibers_list.Count() > 0)
                {
                    current_fiber = fibers_list.Count - 1;
                    Fiber.Switch(fibers_list[current_fiber]);
                }
                else
                {
                    Console.WriteLine("The end");
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                // to allow one process (maybe with lower priority) to get CPU time to work
                uint iter;
                if ((fibers_iter.TryGetValue(iteration, out iter) == true) && (fibers_list.Contains(iter)))
                {
                    current_fiber = fibers_list.IndexOf(iter);
                    Fiber.Switch(iter);
                }
                else
                {
                    current_fiber = ((current_fiber - 1) + fibers_list.Count) % fibers_list.Count;
                    Fiber.Switch(fibers_list[current_fiber]);
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
                fibers_iter.Add((i + 1) * 2, fiber.Id);
                Console.WriteLine("{0}-{1}", fiber.Id, process.Priority);
            }
            fibers = fibers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            current_fiber = fibers.Count - 1;
            fibers_list = fibers.Keys.ToList();
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
