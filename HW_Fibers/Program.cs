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
        public static Queue<uint> queueOfActiveFibers = new Queue<uint>();
        public static List<uint> allFibers = new List<uint>();
        public static Dictionary<uint, int> allFibersWithPriority = new Dictionary<uint, int>();
        public static Dictionary<int, uint> supportDict = new Dictionary<int, uint>();
        private static uint currentFiber;
        //  public static List <System.Collections.Generic.KeyValuePair<int, uint> > allFibersWithPriority = new List<KeyValuePair<int, uint>>();

        /*  private static Dictionary<int, int> fibers = new Dictionary<int, int>(); //fiberId: fiberpriority
          private static Dictionary<int, int> fibersIter = new Dictionary<int, int>(); //number of iter: fiberId
          private static List<int> fibersList = new List<int>();
          private static Queue<int> fibersQueue = new Queue<int>();
          private static int iteration = 0;
          */

        public static void DeleteAllFibers()
        {
            foreach (uint fiber in allFibers)
            {
                 Fiber.Delete(fiber);
            }
        }

        //whitout priority

       public static void Switch(bool fiberFinished)
        {
            if(queueOfActiveFibers.Count == 0)
            {
                DeleteAllFibers();
            }
            queueOfActiveFibers.Dequeue();
            if(fiberFinished)
            {
                if(queueOfActiveFibers.Count == 0)
                {
                    Console.WriteLine("This is the end");
                    Fiber.Switch(Fiber.PrimaryId);
                }
                else
                {
                    currentFiber = queueOfActiveFibers.Peek();
                    Fiber.Switch(currentFiber);
                }
            }
            else
            {
                queueOfActiveFibers.Enqueue(currentFiber);
                currentFiber = queueOfActiveFibers.Peek();
               Fiber.Switch(currentFiber);
            }
        }


        //we sort our fibers by priority and after that we just use FIFO's Switch function
        public static void CreatePriorityForSwitch()
        {
            queueOfActiveFibers.Clear();
            allFibersWithPriority = allFibersWithPriority.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (uint i in allFibersWithPriority.Keys)
            {
                queueOfActiveFibers.Enqueue(i);
            }
            currentFiber = queueOfActiveFibers.Peek();

            Switch(false);
           
        }


        public static void Main()
        {
            int numOfFibers = 5;
            for (int i = 0; i < numOfFibers; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                queueOfActiveFibers.Enqueue(fiber.Id);
                allFibers.Add(fiber.Id);

                allFibersWithPriority.Add(fiber.Id, process.Priority);
                Console.WriteLine("{0}-{1}", fiber.Id, process.Priority);
            }
            //currentFiber = queueOfActiveFibers.Peek(); //the first elem for switch without priority
          

            CreatePriorityForSwitch();
            //Switch(false);
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
