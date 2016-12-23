using System;
using System.Collections.Generic;
using System.Linq;
using Fibers;
using System.Threading;

namespace MyFibers
{
    static class ProcessManager
    {
        private static uint curFiber;
        private static List<uint> fibersId = new List<uint>();
        private static List<uint> fibersForKilling = new List<uint>();
        private static Dictionary<uint, uint> fibersWPriority = new Dictionary<uint, uint>();
        private static Dictionary<uint, uint> fibersWTime = new Dictionary<uint, uint>();
        private static Random rng = new Random();

        public static void DeleteAllFibers()
        {
            foreach (uint fiber in fibersForKilling)
            {
                Console.WriteLine(fiber);
                Fiber.Delete(fiber);                           
            }
        }

        private static uint PickFiberToSwitch()
        {
            uint maxPriorityWMinRatio = 0; // In case of several fibers has equal ratio
            double minRatio = Double.MaxValue; // minRatio = time / (priority + 1) (+1 to aboid division by 0)
            uint answer = 0; // Chosen fiberId
            foreach (uint fiberId in fibersId)
            {
                double curRatio = (double)fibersWTime[fiberId] / (fibersWPriority[fiberId] + 1);
                if (curRatio < minRatio)
                {
                    minRatio = curRatio;
                    maxPriorityWMinRatio = fibersWPriority[fiberId];
                    answer = fiberId;
                }
                else
                {
                    if (curRatio == minRatio)
                    {
                        maxPriorityWMinRatio = Math.Max(fibersWPriority[fiberId], maxPriorityWMinRatio);
                        answer = fiberId;
                    }
                }
            }
            return answer;
        }

        // For choosing next fiber we use rng
        public static void NonPrioritySwitch(bool isFinished)
        {
            if (isFinished)
            {
                Console.WriteLine("Fiber with id {0} has been finished", curFiber);
                fibersId.Remove(curFiber);
                fibersWTime.Remove(curFiber);
                fibersId.Remove(curFiber);
                fibersForKilling.Add(curFiber);
                if (fibersId.Count > 0)
                {
                    uint randomFiber = fibersId[rng.Next(fibersId.Count)];
                    curFiber = randomFiber;
                    Fiber.Switch(curFiber);
                }
                else
                {
                    Console.WriteLine("All fibers finished");
                    fibersId.Clear();
                    fibersWPriority.Clear();
                    fibersWTime.Clear();                    
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                uint randomFiber = fibersId[rng.Next(fibersId.Count)];
                curFiber = randomFiber;
                Fiber.Switch(curFiber);
            }
        }

        // For choosing next fiber we use pickFiberToSwitch()
        public static void PrioritySwitch(bool isFinished)
        {
            if (isFinished)
            {
                Console.WriteLine("Fiber with id {0} has been finished", curFiber);
                fibersWPriority.Remove(curFiber);
                fibersWTime.Remove(curFiber);
                fibersId.Remove(curFiber);
                fibersForKilling.Add(curFiber);
                if (fibersId.Count > 0)
                {
                    uint fiberToSwitch = PickFiberToSwitch();
                    curFiber = fiberToSwitch;
                    fibersWTime[curFiber] += 1;
                    Fiber.Switch(curFiber);
                }
                else
                {
                    Console.WriteLine("All fibers finished");
                    fibersWPriority.Clear();
                    fibersWTime.Clear();
                    fibersId.Clear();
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                uint fiberToSwitch = PickFiberToSwitch();
                curFiber = fiberToSwitch;
                fibersWTime[curFiber] += 1;
                Fiber.Switch(curFiber);
            }
        }

        public static void Main()
        {
            for (int i = 0; i < 5; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                Console.WriteLine("Fiber id: " + fiber.Id + " process priority: " + process.Priority);
                fibersId.Add(fiber.Id);
                fibersWPriority.Add(fiber.Id, (uint)process.Priority);
                fibersWTime.Add(fiber.Id, 0);
            }
            Console.WriteLine("PrimaryId: {0}", Fiber.PrimaryId);
            curFiber = fibersId[0];
            NonPrioritySwitch(false);
            DeleteAllFibers();
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