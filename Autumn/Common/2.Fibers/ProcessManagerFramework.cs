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
        private static SwitchQueue switchQueue = new SwitchQueue();  // queue of processes 

        private static FiberItem FP = new FiberItem(); // Fiber Pointer

        private static int curTime = 0; // loop iterator for distribution cpu time by processes 

        private static int primordialSizeOfQueue; // size of queue after end of pushing processes. it need for distribution cpu time

        // Not depends of priority
        public static void _Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                Console.WriteLine("Fiber " + FP.Id + " has been finished");
                
                // pop this fiber thats why its finished
                switchQueue.Dequeue();

                // Still having any process
                if (switchQueue.Count() > 0)
                {
                    FP = switchQueue.Peek();
                    Fiber.Switch(FP.Id);
                }
                else // if no, going to root process 
                {
                    Console.WriteLine("-----END-----");
                    switchQueue.Clear();
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                // pop from top
                FiberItem first = switchQueue.Dequeue();
                
                // and pushing back
                switchQueue.Enqueue(first);

                // new Fiber Pointer
                FP = switchQueue.Peek();

                //Console.WriteLine("Switch to " + FP);

                // switch to new fiber
                Fiber.Switch(FP.Id);
            }
        }

        // With priority 
        public static void Switch(bool fiberFinished)
        {
            // waiting some mcsec for avoid so fast switching 
            Thread.Sleep(10);

            curTime = (curTime + 1) % (primordialSizeOfQueue * 10); // 10 - lenght of loop
            if (fiberFinished)
            {
                Console.WriteLine("Fiber " + FP.Id + " has been finished");

                // pop this fiber thats why its finished
                switchQueue.Remove(FP);

                // Still any process
                if (switchQueue.Count() > 0)
                {
                    FP = switchQueue.Last();
                    Fiber.Switch(FP.Id);
                }
                else // if no, going to root process 
                {
                    Console.WriteLine("-----END-----");
                    switchQueue.Clear();
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {   
                // low priority process 
                if (switchQueue.TryGetIdByCPUtime(curTime))
                {
                    FP = switchQueue.GetIdByCPUtime(curTime);
                    //Console.WriteLine("Switch to " + FP.Id);
                    Fiber.Switch(FP.Id);
                }
                else // high priority process
                {
                    FP = switchQueue.Last();
                    Fiber.Switch(FP.Id);
                }
            }
        }

        static void Main()
        {
            
            for (int i = 0; i < 10; ++i)
            {
                // creation new process
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));

                Console.WriteLine("Fiber id: " + fiber.Id + " process priority: " + process.Priority);

                // pushing to switcher queue
                switchQueue.Enqueue(new FiberItem(fiber.Id, process.Priority, (i + 1) * 5)); // every 5 time switch to low prior  
            }

            // need for distributon cpu time
            primordialSizeOfQueue = switchQueue.Count();

            // sorting by priority
            switchQueue.Sort();

            // strarting switcher
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