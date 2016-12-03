using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace ProcessManager
{
    struct ProcessInfo
    {
        public uint Id;
        public int Priority;
        public ProcessInfo(uint _Id, int _Priority)
        {
            this.Id = _Id;
            this.Priority = _Priority;
        }
    }

    public static class ProcessManager
    {
 
        private static Queue<ProcessInfo> switchQueue = new Queue<ProcessInfo>();  
        private static uint FP = 0;

        public static void Switch(bool fiberFinished)
        {
            if (switchQueue.Count() == 0)
                return;
            if (fiberFinished)
            {
                Console.WriteLine("Fiber " + FP + " has been finished");
                
                // pop this fiber thats why its finished
                switchQueue.Dequeue();

                // Still having any process
                if (switchQueue.Count() > 0)
                {
                    FP = switchQueue.Peek().Id;
                    Fiber.Switch(FP);
                }
                else // if no, going to root process 
                {
                    Console.WriteLine("-----END-----");
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                // pop from top
                ProcessInfo first = switchQueue.Dequeue();

                // and pushing back
                switchQueue.Enqueue(first);

                // new Fiber Pointer
                FP = switchQueue.Peek().Id;

                Console.WriteLine("Switch to " + FP);

                // switch to new fiber
                Fiber.Switch(FP);
            }
        }


        static void Main()
        {
            Process process1 = new Process();
            Action task1 = new Action(process1.Run);
            Fiber fiber1 = new Fiber(task1);

            Console.WriteLine("Fiber id: " + fiber1.Id + " process priority: " + process1.Priority);

            
            switchQueue.Enqueue(new ProcessInfo(fiber1.Id, process1.Priority));


            Process process2 = new Process();
            Action task2 = new Action(process2.Run);
            Fiber fiber2 = new Fiber(task2);

            Console.WriteLine("Fiber id: " + fiber2.Id + " process priority: " + process2.Priority);

            switchQueue.Enqueue(new ProcessInfo(fiber2.Id, process2.Priority));

            
          
           
            Switch(false);
           

            Console.ReadLine();
        }
    }

    public class Process
    {
        private static readonly Random Rng = new Random();

        private const int LongPauseBoundary = 1000;

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