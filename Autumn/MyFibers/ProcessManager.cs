using System;
using System.Collections.Generic;
using System.Linq;
using Fibers;
using System.Threading;

namespace MyFibers
{
    static class ProcessManager
    {
        private static uint currentFiber;
        private static List<uint> fibersId = new List<uint>();
        private static List<uint> fibersIdForDelete = new List<uint>();
        private static Dictionary<uint, uint> fibersPriority = new Dictionary<uint, uint>();
        private static Dictionary<uint, uint> fibersTime = new Dictionary<uint, uint>();

        public static void DeleteAllFibers() {
            foreach (uint fiber in fibersIdForDelete) {

                Console.WriteLine(fiber);
                if (currentFiber != fiber) {
                    Fiber.Delete(fiber);
                }
            }
        }

        public static void WithoutPrioritySwitch(bool isFinished) {
            if (!isFinished) {
                currentFiber = fibersId.First();
                Fiber.Switch(currentFiber);
            } else {
                Console.WriteLine("Fiber with id {0} has been finished", currentFiber);
                fibersId.Remove(currentFiber);
                fibersTime.Remove(currentFiber);
                if (fibersId.Count > 0) {
                    currentFiber = fibersId.First();
                    Fiber.Switch(currentFiber);
                } else {
                    fibersId.Clear();
                    DeleteAllFibers();
                    Console.WriteLine("All fibers finished");
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
        }

        private static uint getNextFiber() {
            uint max = 0;
            double min = Double.MaxValue;
            uint id = 0;
            foreach (uint fiberId in fibersId) {
                double ratio = (double) fibersTime[fiberId] / (fibersPriority[fiberId] + 1);
                if (min == ratio) {
                    id = fiberId;
                    if (fibersPriority[fiberId] > max) {
                        max = fibersPriority[fiberId];
                    }
                } else if (ratio < min) {
                    id = fiberId;
                    min = ratio;
                    max = fibersPriority[fiberId];
                }
            }
            return id;
        }

        public static void PrioritySwitch(bool isFinished) {
            if (!isFinished) {
                uint nextFiber = getNextFiber();
                currentFiber = nextFiber;
                fibersTime[nextFiber] += 1;
                Fiber.Switch(nextFiber);
            } else {
                Console.WriteLine("Fiber with id {0} has been finished", currentFiber);
                fibersId.Remove(currentFiber);
                fibersTime.Remove(currentFiber);
                fibersPriority.Remove(currentFiber);
                if (fibersId.Count == 0) {
                    Console.WriteLine("All fibers finished");
                    fibersPriority.Clear();
                    fibersTime.Clear();

                    Console.WriteLine("All fibers finished");
                    DeleteAllFibers();

                    Console.WriteLine("All fibers finished");
                    Fiber.Switch(Fiber.PrimaryId);
                } else {
                    uint nextFiber = getNextFiber();
                    currentFiber = nextFiber;
                    fibersTime[nextFiber] += 1;
                    Fiber.Switch(nextFiber);
                }
            }
        }

        public static void Main() {
            for (int i = 0; i < 7; i++) {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                Console.WriteLine("ID fiber: " + fiber.Id + " process priority: " + process.Priority);
                fibersId.Add(fiber.Id);
                fibersIdForDelete.Add(fiber.Id);
                fibersPriority.Add(fiber.Id, (uint)process.Priority);
                fibersTime.Add(fiber.Id, 0);                      
            }
            currentFiber = fibersId[0];
            PrioritySwitch(false);
            Console.ReadLine();
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
                        ProcessManager.PrioritySwitch(false);
                    } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
                }
                ProcessManager.PrioritySwitch(true);
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
}
