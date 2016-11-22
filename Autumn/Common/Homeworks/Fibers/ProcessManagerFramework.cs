using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Fibers
{
    public static class ProcessManager
    {
        public static List<Process> Processes = new List<Process>();
        public static List<uint> FibersList = new List<uint>();
        public static List<uint> FibersToDelete = new List<uint>();
        public static int CurrentProcess;
        public static uint CurrentFiber;
        public static int Counter = 0;
        public static int ShareWithLowFiber = 3;
        public static Random Rnd = new Random();

        private static void Delete()
        {
            foreach (uint fib in FibersToDelete)
            {
                if((fib != Fiber.PrimaryId) && (fib != CurrentFiber))
                {
                    Fiber.Delete(fib);
                }
            }
            Console.WriteLine("All fibers have been deleted!");
            CurrentFiber = Fiber.PrimaryId;
        }

        private static void DeleteOneFiber()
        {
            if(CurrentFiber != Fiber.PrimaryId)
            {
                uint fiberToDelete = CurrentFiber;
                FibersList.Remove(fiberToDelete);
                Processes.Remove(Processes[CurrentProcess]);
                CurrentProcess = 0;
            }
        }

        /*
        // unpriority 
        public static void Switch(bool fiberFinished)
        {
            if(fiberFinished)
            {
                DeleteOneFiber();
            }
   
            if(FibersList.Count > 1)
            {
                for (int i = 0; i < FibersList.Count / 2; i++)
                {
                    Process tmpProcess = Processes[i];
                    Processes[i] = Processes[Processes.Count - 1 - i];
                    Processes[Processes.Count - 1 - i] = tmpProcess;
                    uint tmpFiber = FibersList[i];
                    FibersList[i] = FibersList[FibersList.Count - 1 - i];
                    FibersList[FibersList.Count - 1 - i] = tmpFiber;
                }
                CurrentFiber = FibersList[FibersList.Count - 1];
                Console.WriteLine("Fiber has been switched!");
            }
            else
            {
                Delete();
            }
            Fiber.Switch(CurrentFiber);
        }
        */
        
        // priority
        public static void Switch(bool fiberFinished)
        {
            if(fiberFinished)
            {
                DeleteOneFiber();
            }

            if(FibersList.Count > 1)
            {
                Counter++;
                Process maxPriorityProcess = new Process();
                int maxPriority = Int32.MinValue;
                foreach (Process proc in Processes)
                {
                    if (proc != Processes[CurrentProcess] && proc.Priority > maxPriority)
                    {
                        maxPriority = proc.Priority;
                        maxPriorityProcess = proc;
                    }
                }

                if (Counter == ShareWithLowFiber)
                {
                    Counter = 0;
                    int tmp = Rnd.Next(0, Processes.Count - 1);
                    CurrentProcess = tmp;
                    CurrentFiber = FibersList[CurrentProcess];
                    Console.WriteLine("Switched on the Fiber with priority {0}", Processes[CurrentProcess].Priority);
                }
                else
                {
                    CurrentProcess = Processes.IndexOf(maxPriorityProcess);
                    CurrentFiber = FibersList[CurrentProcess];
                    Console.WriteLine("Switched on the Fiber with priority {0}", Processes[CurrentProcess].Priority);
                }
            }
            else
            {
                Delete();
            }
            Fiber.Switch(CurrentFiber);
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
