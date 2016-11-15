using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace ProcessManager
{
    public class Process
    {
        private static Random Rnd = new Random();
        private const int MaxDur = 1000;
        private const int MaxNumIter = 5;
        private const int MaxPriority = 100;
        public int NumIter;
        public int Priority;

        public Process()
        {
            NumIter = Rnd.Next(MaxNumIter) + 1;
            Priority = Rnd.Next(MaxPriority);
        }

        public void Run()
        {
            for (int i = 0; i < NumIter; i++)
            {
                Console.WriteLine(string.Format("Fiber {0} now working on iter {1} with priority {2}", ProcessManager.CurFiber, i, Priority));

                // work emulation
                int workDuration = Rnd.Next(MaxDur);
                ProcessManager.IsIncrease = false;
                Thread.Sleep(workDuration);

                // I/O emulation
                int pauseDuration = Rnd.Next(MaxDur);
                DateTime pauseBeginTime = DateTime.Now;
                do
                {
                    Console.WriteLine(string.Format("Fiber {0} now input or output with {1} mls", ProcessManager.CurFiber, pauseDuration - (DateTime.Now - pauseBeginTime).TotalMilliseconds));
                    ProcessManager.Switch();
                } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < pauseDuration);
            }
            ProcessManager.Switch();
        }
    }

    class ProcessManager
    {
        private static Random Rnd = new Random();
        // Priority algorithm or not
        private static bool IsModePriority = true; // Now its priority algorithm
        private static bool IsStart = true;
        // Lists of indexes of fibers
        private static List<uint> FibersList = new List<uint>();
        private static List<uint> CopyList = new List<uint>();
        // Id of fiber to number of iteration 
        private static Dictionary<uint, int> IdToIter = new Dictionary<uint, int>();
        // Id of fiber to current iteration
        private static Dictionary<uint, int> IterOnId = new Dictionary<uint, int>();
        // Id of fiber to current iteration
        private static Dictionary<uint, int> IdToPrior = new Dictionary<uint, int>();
        // List of fibers which sorted by priority
        private static SortedSet<Tuple<int, uint>> PriorityQueue = new SortedSet<Tuple<int, uint>>();

        public static uint CurFiber = 0;
        public static bool IsIncrease;

        private static uint GetNextFiber()
        {
            uint nextFiber;
            // 20% chance to switch process to low priority
            if (Rnd.Next(100) < 20) 
            {
                nextFiber = PriorityQueue.First().Item2;
                return nextFiber;
            }

            if (PriorityQueue.Count() > 1)
            {
                PriorityQueue.Remove(new Tuple<int, uint>(IdToPrior[CurFiber], CurFiber));
            }
            nextFiber = PriorityQueue.Last().Item2;
            PriorityQueue.Add(new Tuple<int, uint>(IdToPrior[CurFiber], CurFiber));
            return nextFiber;
        }

        public static void Switch()
        {
            Thread.Sleep(5);
            if (!IsStart && IterOnId[CurFiber] == IdToIter[CurFiber])
            {
                Console.WriteLine(string.Format("Fiber {0} has finished", CurFiber));
                if (IsModePriority)
                {
                    PriorityQueue.Remove(new Tuple<int, uint>(IdToPrior[CurFiber], CurFiber));
                }
                else
                {
                    FibersList.Remove(CurFiber);
                }
                if (FibersList.Count() == 0 || PriorityQueue.Count() == 0)
                {
                    Fiber.Switch(Fiber.PrimaryId);
                    for (int i = 0; i < CopyList.Count(); i++)
                    {
                        Fiber.Delete(CopyList[i]);
                    }
                    return;
                }
                if (IsModePriority)
                {
                    CurFiber = PriorityQueue.Last().Item2;
                }
                else
                {
                    CurFiber = FibersList.First();
                }
                Fiber.Switch(CurFiber);
            }
            else
            {
                if (!IsStart)
                {
                    Console.WriteLine(string.Format("Fiber {0} has stopped", CurFiber));
                    if (IsIncrease == false)
                    {
                        IterOnId[CurFiber]++;
                        IsIncrease = true;
                    }
                    if (IsModePriority)
                    {
                        CurFiber = GetNextFiber();
                    }
                    else
                    {
                        FibersList.Add(CurFiber);
                        FibersList.Remove(CurFiber);
                        CurFiber = FibersList.First();
                    }
                }
                else if (IsModePriority)
                {
                    CurFiber = PriorityQueue.Last().Item2;
                }
                else
                {
                    CurFiber = FibersList.First();
                }
                IsStart = false;
                Fiber.Switch(CurFiber);
            }

        }

        static void Main(string[] args)
        {
            // number of process
            uint numProc = 5;

            for (int i = 0; i < numProc; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                uint id = fiber.Id;
                FibersList.Add(id);
                CopyList.Add(id);
                IdToIter.Add(id, process.NumIter);
                IterOnId.Add(id, 0);
                IdToPrior.Add(id, process.Priority);
                PriorityQueue.Add(new Tuple<int, uint>(process.Priority, id));
            }

            Switch();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
