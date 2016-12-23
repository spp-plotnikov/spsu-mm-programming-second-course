using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab2
{
    public static class ProcessManager
    {
        public static List<uint> Fibers = new List<uint>();
        static List<uint> _fibersForDelete = new List<uint>();
        public static List<Process> Processes = new List<Process>();
        public static uint CurFiber;
        static int _index = 0;
        public static bool Priority;
        static int _step = 4;
        static double _rank;

        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                _fibersForDelete.Add(CurFiber);
                Fibers.Remove(CurFiber);
                Processes.Remove(Processes[_index]);
            }
            if (Fibers.Count < 1)
            {
                Console.WriteLine("The end");
                Fiber.Switch(Fiber.PrimaryId);
            }
            else
            {
                _index = Priority ? GetFiber() : GetRandomFiber();
                CurFiber = Fibers[_index];
                Console.WriteLine("Switch to another fiber");
                Fiber.Switch(CurFiber);
            }
            Thread.Sleep(100);
        }

        private static int GetRandomFiber()
        {
            Random rand = new Random();
            Process newProcess = Processes.ElementAt(rand.Next(Processes.Count));
            return Processes.IndexOf(newProcess);
        }

        private static int GetFiber()
        {
            GetRank();
            List<Process> suitableFibers = new List<Process>();
            Process newProcess = new Process();
            Random rand = new Random();
            if (_step != 4)
            {
                _step++;
                foreach (var proc in Processes)
                {

                    if (proc.Priority >= _rank)
                    {
                        suitableFibers.Add(proc);
                    }
                }
            }
            else
            {
                _step = 0;
                foreach (var proc in Processes)
                {
                    if (proc.Priority <= _rank)
                    {
                        suitableFibers.Add(proc);
                    }
                }
            }
            if (suitableFibers.Count > 0)
            {
                newProcess = suitableFibers.ElementAt(rand.Next(suitableFibers.Count));
                return Processes.IndexOf(newProcess);
            }
            else
            {
                return GetMaxFiber();
            }
        }

        private static int GetMaxFiber()
        {
            Process newProcess = new Process();
            int maxPrior = Int32.MinValue;
            foreach (Process proc in Processes)
            {
                if (maxPrior < proc.Priority && proc != Processes[_index])
                {
                    newProcess = proc;
                    maxPrior = proc.Priority;
                }
            }
            return Processes.IndexOf(newProcess);
        }

        private static void GetRank()
        {
            _rank = 0;
            foreach (var proc in Processes)
            {
                _rank += proc.Priority;
            }
            _rank = _rank / (double)Processes.Count;
        }

        public static void DeleteAll()
        {
            foreach (var fiber in _fibersForDelete)
            {
                if (fiber != Fiber.PrimaryId)
                {
                    Fiber.Delete(fiber);
                }
            }
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
