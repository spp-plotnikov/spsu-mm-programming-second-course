using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Fibers
{
    public static class ProcessManager
    {
        private static List<Tuple<uint, int>> _fibers = new List<Tuple<uint, int>>();
        private static Tuple<uint, int> _curFiber;
        public static bool IsPriorityEnabled;
        private static int _prioritiesSum;
        private static Random _rng = new Random();

        public static bool AddFiber(Process process)
        {
            try
            {
                Fiber fiber = new Fiber(process.Run);
                _fibers.Add(new Tuple<uint, int>(fiber.Id, process.Priority));
                _curFiber = _fibers.Last();
                _prioritiesSum = _fibers.Sum(x => x.Item2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static void Switch(bool isFiberFinished)
        {
            if (isFiberFinished)
            {
                _fibers.Remove(_curFiber);
                Console.WriteLine(_curFiber.Item1 + " finished.");
                if (_fibers.Count == 0)
                {
                    try
                    {
                        Fiber.Switch(Fiber.PrimaryId);
                    }
                    catch
                    {
                        //
                    }
                    return;
                }
            }
            Fiber.Switch(GetNextFiberId());
        }

        private static uint GetNextFiberId()
        {
            if (IsPriorityEnabled)
            {
                var highRated = _fibers.Where(x => x.Item2 >= _prioritiesSum / _fibers.Count).ToList();
                if (highRated.Any())
                {
                    _curFiber = highRated.ElementAt(_rng.Next(highRated.Count - 1));
                    _prioritiesSum -= _curFiber.Item2;
                    if (_prioritiesSum == 0)
                    {
                        _prioritiesSum = _fibers.Sum(x => x.Item2);
                    }
                    return _curFiber.Item1;
                }
            }
            _curFiber = _fibers[_rng.Next(_fibers.Count - 1)];
            return _curFiber.Item1;
        }
    }

    internal class Comparer : IComparer<Tuple<uint, int>>
    {
        public int Compare(Tuple<uint, int> x, Tuple<uint, int> y)
        {
            if (x.Item2 > y.Item2)
                return -1;
            if (x.Item2 < y.Item2)
                return 1;
            return 0;
        }
    }
}
