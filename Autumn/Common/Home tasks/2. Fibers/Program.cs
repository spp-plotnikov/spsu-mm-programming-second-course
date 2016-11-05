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
        private int duration;
        private static Random Rnd = new Random();
        private static int durInt = 1000;
        private static int maxNumIter = 5;


        public int numIter;
        public Process()
        {
            duration = Rnd.Next(durInt) + 50;
            numIter = Rnd.Next(maxNumIter) + 1;
        }
        public void Run()
        {
            for (int i = 0; i < numIter; i++)
            {
                Console.WriteLine(string.Format("Fiber {0} now working on iter {1}", ProcessManager.curFiber, i));
                Thread.Sleep(duration);
                ProcessManager.Switch();
            }
            ProcessManager.Switch();
        }
    }


    class ProcessManager
    { 
        // Priority algorithm or not
        public static bool isModePriority;
        // List of indexes of fibers
        public static List<uint> fibersList = new List<uint>();
        public static List<uint> copyList = new List<uint>();
        // Id of fiber to number of iteration 
        public static Dictionary<uint, int> idToIter = new Dictionary<uint, int>();
        // Id of fiber to current iteration
        public static Dictionary<uint, int> iterOnId = new Dictionary<uint, int>();
        public static uint curFiber = 0;
        private static bool isStart;

        public static void Switch()
        {

            if (isModePriority)
            {

            }
            else
            {
                if (!isStart && iterOnId[curFiber] == idToIter[curFiber])
                {
                    Console.WriteLine(string.Format("Fiber {0} has finished", curFiber));
                    fibersList.Remove(curFiber);
                    if (fibersList.Count() == 0)
                    {
                        Fiber.Switch(Fiber.PrimaryId);
                        for (int i = 0; i < copyList.Count(); i++)
                        {
                            Fiber.Delete(copyList[i]);
                        }
                        return;
                    }
                    curFiber = fibersList.First();
                    Fiber.Switch(curFiber);
                }
                else
                {
                    if (!isStart)
                    {
                      Console.WriteLine(string.Format("Fiber {0} has stopped", curFiber));
                        fibersList.Add(curFiber);
                        iterOnId[curFiber]++;
                        fibersList.Remove(curFiber);
                    }
                    isStart = false;
                    curFiber = fibersList.First();
                    Fiber.Switch(curFiber);
                }
            }
        }
  
        static void Main(string[] args)
        {

            // set mode to Priority or not
            isModePriority = false;
            isStart = true;
            // number of process
            uint numProc = 5;

            for (int i = 0; i < numProc; i++)
            {
                Process process = new Process();
                Fiber fiber = new Fiber(new Action(process.Run));
                fibersList.Add(fiber.Id);
                idToIter.Add(fiber.Id, process.numIter);
                iterOnId.Add(fiber.Id, 0);
                copyList.Add(fiber.Id);
            }

            Switch();

            Console.WriteLine("Done");
        }
    }
}
