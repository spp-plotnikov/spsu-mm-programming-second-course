using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace ProcessManager
{
    public static class MyProcessManager
    {
        private static Random _rng = new Random();
        private static int _num;
        private static Queue<int> _fibersInQueue = new Queue<int>();
        private static List<double> _listForRandom = new List<double>();
        private static uint[] _fibersId;
        private static int _curSumPriorities = 0;
        private static Dictionary<int, int> _fibersAndPriorities = new Dictionary<int, int>();
        private static int _curFiber;


        private static void DeleteAll ()
        {
            foreach (uint id in _fibersId)
            {
                Fiber.Delete(id);
                Console.WriteLine(id.ToString() + " Delete");
            }
        }
        public static void SwitchWithoutPriority(bool fiberFinished)
        {
            if (fiberFinished) //файбер закончил процесс
            {
                Console.WriteLine(_fibersInQueue.Dequeue().ToString() + " Finish");

                if (_fibersInQueue.Count == 0)
                {
                    Console.WriteLine("TheEnd");
                    Fiber.Switch(Fiber.PrimaryId);
                }
                else
                {
                    Fiber.Switch(_fibersId[_fibersInQueue.Peek()]);
                }
            }
            else //переложить первый файбер в очереди в конец очереди
            {
                _fibersInQueue.Enqueue(_fibersInQueue.Dequeue());
                Fiber.Switch(_fibersId[_fibersInQueue.Peek()]);
            }
        }

        private static void CreateListForRandom ()
        {
            _listForRandom.Clear();
            double curAccum = 0;
            foreach (int prior in _fibersAndPriorities.Values)
            {
                curAccum += (double)prior / _curSumPriorities;
                _listForRandom.Add(curAccum);
            }
            _listForRandom[_listForRandom.Count - 1] = 1.0;
        }

        private static int getRandomFiber ()
        {
            double rand = _rng.NextDouble();
            for (int i = 0; i < _listForRandom.Count; i++)
            {
                if (_listForRandom[i] > rand)
                {
                    return _fibersAndPriorities.Keys.ToArray()[i];
                }
            }
            return _fibersAndPriorities.Keys.ToArray()[0]; //если что-то и пошло не так - то вызываем самый приоритетный
        }

        public static void SwitchWithPriority(bool fiberFinished)
        {
            if (fiberFinished) //файбер закончил процесс
            {
                Console.WriteLine(_curFiber + " Finish");
                _curSumPriorities -= _fibersAndPriorities[_curFiber]; //чтобы каждый раз не пересчитывать сумму
                _fibersAndPriorities.Remove(_curFiber);
                if (_fibersAndPriorities.Count == 0)
                {
                    Console.WriteLine("TheEnd");
                    Fiber.Switch(Fiber.PrimaryId);
                }
                else
                {
                    CreateListForRandom(); //перестраиваем                 
                    _curFiber = getRandomFiber();
                    Fiber.Switch(_fibersId[_curFiber]);
                }
            }
            else
            {
                _curFiber = getRandomFiber();
                Fiber.Switch(_fibersId[_curFiber]);
            }
        }

        public static void Emulation (int numOfFibers, bool isPriority)
        {
            _num = numOfFibers;
            List<uint> fibers = new List<uint>();
            if (isPriority)
            {
                for (int i = 0; i < _num; i++)
                {
                    Process process = new Process(isPriority);
                    Fiber fiber = new Fiber(new Action(process.Run));
                    fibers.Add(fiber.Id);
                    _fibersAndPriorities.Add(i, process.Priority);
                    Console.WriteLine(i.ToString() + " id" + fiber.Id.ToString() + " Start");
                }
                _fibersId = fibers.ToArray();
                _fibersAndPriorities = _fibersAndPriorities.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (int prior in _fibersAndPriorities.Values)
                {
                    _curSumPriorities += prior;
                }
                CreateListForRandom();
                SwitchWithPriority(false);
            }
            else
            {
                for (int i = 0; i < _num; i++)
                {
                    Process process = new Process(isPriority);
                    Fiber fiber = new Fiber(new Action(process.Run));
                    fibers.Add(fiber.Id);
                    _fibersInQueue.Enqueue(i);
                    Console.WriteLine(i.ToString() + " id" + fiber.Id.ToString() + " Start");
                }
                _fibersId = fibers.ToArray();
                SwitchWithoutPriority(false);
            }
            DeleteAll();
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

        private readonly bool _isPriority;

        private readonly List<int> _workIntervals = new List<int>();
        private readonly List<int> _pauseIntervals = new List<int>();

        public Process (bool isPriority)
        {
            int amount = Rng.Next(IntervalsAmountBoundary);
            _isPriority = isPriority;
            for (int i = 0; i < amount; i++)
            {
                _workIntervals.Add(Rng.Next(WorkBoundary));
                _pauseIntervals.Add(Rng.Next(
                        Rng.NextDouble() > 0.9
                            ? LongPauseBoundary
                            : ShortPauseBoundary));
            }
			
			Priority = Rng.Next(PriorityLevelsNumber) + 1;
        }

        public void Run()
        {
            for (int i = 0; i < _workIntervals.Count; i++)
            {
                Thread.Sleep(_workIntervals[i]); // work emulation
                DateTime pauseBeginTime = DateTime.Now;
                if (_isPriority)
                {
                    do
                    {
                        MyProcessManager.SwitchWithPriority(false);
                    } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
                    MyProcessManager.SwitchWithPriority(true);
                }
                else
                {
                    do
                    {
                        MyProcessManager.SwitchWithoutPriority(false);
                    } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
                    MyProcessManager.SwitchWithoutPriority(true);
                }
            }
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
