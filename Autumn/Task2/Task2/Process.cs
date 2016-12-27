using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    public class Process
    {
        public int Priority
        {
            get;
            private set;
        }
        private int size;

        public Process(int priority, int size)
        {
            Priority = priority;
            if(Priority < 0)
            {
                Priority = 0;
            }
            if(Priority > 9)
            {
                Priority = 9;
            }
            this.size = size;
        }

        public void Run()
        {
            Random random = new Random();
            int pivot = 10;
            bool isDone = false;
            int workingTime = (random.Next() % 10 + 1) * 1000;
            int waitingTime = random.Next() % 5 + 1;
            while(size > 0)
            {
                if(random.Next(100) > pivot)
                {
                    Thread.Sleep(workingTime);
                    size--;
                }
                else
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    while(s.Elapsed < TimeSpan.FromSeconds(waitingTime))
                    {
                        ProcessManagerFramework.Switch(isDone);
                    }
                    s.Stop();
                }
            }
            isDone = true;
            ProcessManagerFramework.Switch(isDone);
        }
    }
}
