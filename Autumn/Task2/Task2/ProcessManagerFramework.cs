using Fibers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public static class ProcessManagerFramework
    {
        public static bool PriorityEnabled;
        private static List<Tuple<Fiber, int>> fibers;
        private static int onGoingFiber = 0;

        static ProcessManagerFramework()
        {
            PriorityEnabled = false;
            fibers = new List<Tuple<Fiber, int>>();
        }

        public static void AddFiber(Fiber fiber, Process process)
        {
            fibers.Add(new Tuple<Fiber, int>(fiber, process.Priority));
        }

        public static void Clear()
        {
            foreach(var fiber in fibers)
            {
                fiber.Item1.Delete();
            }
        }

        public static void Switch(bool isDone)
        {
            switch(PriorityEnabled)
            {
                case true:
                    PrioritySwitch(isDone);
                    break;
                case false:
                    SimpleSwitch(isDone);
                    break;
            }
        }

        private static void PrioritySwitch(bool isDone)
        {
            if(fibers.Count() == 0)
            {
                Clear();
                return;
            }
            int next = Distribution();
            if(isDone)
            {
                fibers.RemoveAt(onGoingFiber);
                if(fibers.Count() > 0)
                {
                    onGoingFiber = next;
                    Fiber.Switch(fibers[onGoingFiber].Item1.Id);
                }
                else
                {
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                Tuple<Fiber, int> temp = fibers[next];
                fibers[next] = fibers[0];
                fibers[0] = temp;
                onGoingFiber = 0;
                Fiber.Switch(fibers[onGoingFiber].Item1.Id);
            }
        }

        private static int Distribution()
        {
            int[] priors = new int[10];
            foreach(var fiber in fibers)
            {
                priors[fiber.Item2] = 1;
            }
            int point = new Random().Next(1024) + 1;
            int result = 0;
            for(int i = 0; i <= 10; i++)
            {
                if(point >= Math.Pow(2, i) && priors[i] == 1)
                {
                    result = i;
                    break;
                }
            }
            List<Tuple<Fiber, int>> temp = new List<Tuple<Fiber, int>>();
            foreach(var fiber in fibers)
            {
                if(fiber.Item2 == result)
                {
                    temp.Add(fiber);
                }
            }
            point = new Random().Next(temp.Count);
            return fibers.IndexOf(temp[point]);
        }

        private static void SimpleSwitch(bool isDone)
        {
            if(fibers.Count() == 0)
            {
                Clear();
                return;
            }

            if(isDone)
            {
                fibers.RemoveAt(onGoingFiber);
                if(fibers.Count() > 0)
                {
                    onGoingFiber = 0;
                    Fiber.Switch(fibers[onGoingFiber].Item1.Id);
                }
                else
                {
                    Fiber.Switch(Fiber.PrimaryId);
                }
            }
            else
            {
                Tuple<Fiber, int> temp = fibers.Last();
                fibers[fibers.Count - 1] = fibers[0];
                fibers[0] = temp;
                onGoingFiber = 0;
                Fiber.Switch(fibers[onGoingFiber].Item1.Id);
            }
        }
    }
}
