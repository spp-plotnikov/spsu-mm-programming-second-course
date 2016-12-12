using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab2
{
    public static class ProcessManager
    {
        static List<uint> _fibers = new List<uint>();
        static Queue<int> _fibersQueue = new Queue<int>();
        static List<Process> _processes = new List<Process>();
        static uint _curFiber;
        static int _index;
        static bool _priority;

        public static void Do(int numberOfFibers, bool priority)
        {
            _priority = priority;
            for (int i = 0; i < numberOfFibers; i++)
            {
                Process proc = new Process();
                _processes.Add(proc);
                Fiber fiber = new Fiber(new Action(proc.Run));
                _fibers.Add(fiber.Id);
                _fibersQueue.Enqueue(i);
            }
            _curFiber = _fibers.First();
            _index = 0;
        }

        public static void Switch(bool fiberFinished)
        {
            if (_priority)
            {
                if (fiberFinished)
                {
                    if (_curFiber != Fiber.PrimaryId)
                    {
                        _fibers.Remove(_curFiber);
                        _processes.Remove(_processes[_index]);
                        _index = 0;
                    }
                }
                if (_fibers.Count <= 1)
                {
                    DeleteAll();
                    _curFiber = Fiber.PrimaryId;
                    Console.WriteLine("The end");
                    Thread.Sleep(50);
                    Fiber.Switch(_curFiber);
                }
                else
                {
                    _index = GetFiber();
                    _curFiber = _fibers[_index];
                    Console.WriteLine("Switch to another fiber");
                    Thread.Sleep(50);
                    Fiber.Switch(_curFiber);
                }
            }
            else
            {
                if (fiberFinished)
                {
                    _fibersQueue.Dequeue();
                }
                if (_fibersQueue.Count <= 0)
                {
                    Console.WriteLine("The end");
                    _curFiber = Fiber.PrimaryId;
                    Thread.Sleep(50);
                    Fiber.Switch(_curFiber);
                }
                else
                {
                    Console.WriteLine("Switch to another fiber");
                    int tempFirstFiber = _fibersQueue.Dequeue();
                    _fibersQueue.Enqueue(tempFirstFiber);
                    _curFiber = _fibers[_fibersQueue.Peek()];
                    Thread.Sleep(50);
                    Fiber.Switch(_curFiber);
                }
            }
        }

        private static int GetFiber()
        {
            Process newProcess = new Process();
            Process curProcess = _processes[_index];
            int maxPrior = Int32.MinValue;
            foreach (Process proc in _processes)
            {
                if (proc != curProcess && maxPrior < proc.Priority)
                {
                    newProcess = proc;
                    maxPrior = proc.Priority;
                }
            }
            return _processes.IndexOf(newProcess);
        }

        private static void DeleteAll()
        {
            foreach (var fiber in _fibers)
            {
                Fiber.Delete(fiber);
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
