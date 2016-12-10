using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;
using System.Collections;

namespace ProcessManager
{
    public static class ProcessManager
    {
        /// <summary>
        /// Support struct for info representaion.
        /// </summary>
        private struct FibInfo
        {
            public uint Id;
            public int Priority;

            public FibInfo(uint id, int priority)
            {
                Id = id;
                Priority = priority;
            }
        };

        private static bool _mode = false;
        private static uint _count = 0;
        private static uint _curFib;
        private static Alias _rnd = new Alias();
        /// <summary>
        /// Queue for non-priority representation
        /// </summary>
        private static Queue<FibInfo> _fibQueue = new Queue<FibInfo>();

        /// <summary>
        /// Global emergency storage
        /// </summary>
        private static FibInfo[] _storage;

        /// <summary>
        /// Id & Priority dictionary.
        /// </summary>
        /// <remarks> For priority mode is rather useful to manage dictionary, then array or queue. 
        ///  It is determined by the specifics of the random generator.</remarks>
        private static Dictionary<uint, int> _fibPrior = new Dictionary<uint, int>();

        /// <summary>
        /// Main fiber magic place - switch between fibers. 
        /// </summary>
        /// <remarks> Mode sets in Emulation function. </remarks>
        /// <param name="fiberFinished"> Is fiber finished </param>
        public static void Switch(bool fiberFinished)
        {
            if (!_mode) //without priority 
            {
                if (fiberFinished)
                {
                    Console.WriteLine(_fibQueue.Dequeue().Id.ToString() + " finished");
                    _count--;
                    
                    if (_count == 0) // all is done
                    {
                        Console.WriteLine("======");
                        Fiber.Switch(Fiber.PrimaryId);  //back to Emulation function
                    }
                    else
                        Fiber.Switch(_fibQueue.Peek().Id);
                }

                else // job isn't done - go to the end
                {
                    _fibQueue.Enqueue(_fibQueue.Dequeue());
                    Fiber.Switch(_fibQueue.Peek().Id);

                }
            }
            else // priority mode
            {
                if (fiberFinished)
                {
                    Console.WriteLine(_curFib + " Finish");

                    _fibPrior.Remove(_curFib);
                    _count--;

                    if (_count == 0) // all is done
                    {
                        Console.WriteLine("======");
                        Fiber.Switch(Fiber.PrimaryId);  //back to Emulation function
                    }
                    else
                    {
                        _rnd.Delete(_curFib);
                        _curFib = (uint)_rnd.Get();
                        Fiber.Switch(_curFib);
                    }

                }
                else // just get next
                {
                    _curFib = (uint)_rnd.Get();
                    Fiber.Switch(_curFib);
                }

            }
        }
        /// <summary>
        /// Main emulation function.
        /// </summary>
        /// <param name="num"> Number of fibers in thread </param>
        /// <param name="mode"> Priority Switch or not </param>
        public static void Emulation(uint num, bool mode)
        {
            

            /* Initialisation */
            _mode = mode;            
            _count = num;
            _storage = new FibInfo[num];

            for (int i = 0; i < num; i++)
            {                                
                Process proc = new Process();
                Fiber fib = new Fiber(new Action(proc.Run));
                _storage[i].Id = fib.Id;
                _storage[i].Priority = mode ? proc.Priority : 0;
                
                Console.WriteLine("{0} - {1} started", _storage[i].Id, _storage[i].Priority);

                if(!_mode)
                 _fibQueue.Enqueue(new FibInfo(_storage[i].Id, _storage[i].Priority));
                else
                {
                    _fibPrior.Add(_storage[i].Id, _storage[i].Priority);
                    _rnd.Add(_storage[i].Id, _storage[i].Priority);
                }
            }
            /* Execution */ 
            Switch(false);

            /* End */ 
            DeleteAll();
        }

        public static void DeleteAll()
        {
            foreach (FibInfo i in _storage)
            {
                Fiber.Delete(i.Id);
                Console.WriteLine("{0} deleted", i.Id);
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

            Priority = Rng.Next(PriorityLevelsNumber) + 1; ///CHANGED!!!
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
