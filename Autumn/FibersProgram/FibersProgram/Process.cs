using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FibersProgram
{
    /// <summary>
    /// Содержит модель процесса операционной системы, где периоды работы сменяются периодами операций ввода-вывода
    /// </summary>
    ///<remarks> При этом априори невозможно узнать, какова будет длительность этих интервалов.
    /// В операционных системах Windows 3.1 и ранних MacOS не было понятия вытесняющей многозадачности.
    /// Вместо этого управление отдавалось другому процессу добровольно и вручную – например, когда процесс 
    /// находился в ожидании операции ввода-вывода. Это реализовано в модели процесса. </remarks>
    public class Process
    {
        private static readonly Random Rng = new Random();

        private const int LongPauseBoundary = 10000;

        private const int ShortPauseBoundary = 100;

        private const int WorkBoundary = 1000;

        private const int IntervalsAmountBoundary = 10;

        private const int PriorityLevelsNumber = 10;

        private bool priority;

        private readonly List<int> _workIntervals = new List<int>();
        private readonly List<int> _pauseIntervals = new List<int>();

        public Process(bool priority)
        {
            int amount = Rng.Next(IntervalsAmountBoundary);
            this.priority = priority;
            for (int i = 0; i < amount; i++)
            {
                _workIntervals.Add(Rng.Next(WorkBoundary));
                _pauseIntervals.Add(Rng.Next(
                        Rng.NextDouble() > 0.9
                            ? LongPauseBoundary
                            : ShortPauseBoundary));
            }

            if (priority)
                Priority = Rng.Next(PriorityLevelsNumber);
        }

        public void Run()
        {
            Console.WriteLine("In run process");

            for (int i = 0; i < _workIntervals.Count; i++)
            {
                Thread.Sleep(_workIntervals[i]); // work emulation
                DateTime pauseBeginTime = DateTime.Now;
                do
                {
                    ProcessManagerFramework.Switch(false);
                } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
            }
            ProcessManagerFramework.Switch(true);
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
