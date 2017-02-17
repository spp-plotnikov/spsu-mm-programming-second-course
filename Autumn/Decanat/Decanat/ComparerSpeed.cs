using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    class ComparerSpeed
    {
        private system _system;
        private List<Task> tasks = new List<Task>();

        public ComparerSpeed(system _system)
        {
            this._system = _system;
        }

        public void PrintSpeedResult()
        {
            DateTime timer = DateTime.Now;
            if(_system == system.SystemCoarseGrained)
            {
                SystemCoarseGrained scg = new SystemCoarseGrained();
                for (int i = 0; i < 90; i++)
                {
                    int j = i;
                    tasks.Add(new Task(() => scg.Add(3210001 + j, 1, job.Teacher)));
                }
                for (int i = 0; i < 10; i++)
                {
                    int j = i;
                    tasks.Add(new Task(() => scg.Remove(3210001 + j, 1, job.Teacher)));
                }
                for (int t = 0; t < 10; t++)
                {
                    for (int i = 0; i < 90; i++)
                    {
                        int j = i;
                        tasks.Add(new Task(() => scg.Contains(3210001 + j, 1)));
                    }
                }
                foreach (Task task in tasks)
                    task.Start();
                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("Time of work {0}: ", _system);
                Console.WriteLine(DateTime.Now - timer);
            }
            else
            {
                SystemSpinLock ssl = new SystemSpinLock();
                for (int i = 0; i < 90; i++)
                {
                    int j = i;
                    tasks.Add(new Task(() => ssl.Add(3210001 + j, 1, job.Teacher)));
                }
                for (int i = 0; i < 10; i++)
                {
                    int j = i;
                    tasks.Add(new Task(() => ssl.Remove(3210001 + j, 1, job.Teacher)));
                }
                for (int i = 0; i < 900; i++)
                {
                    int j = i % 90;
                    tasks.Add(new Task(() => ssl.Contains(3210001 + j, 1)));
                }
                foreach (Task task in tasks)
                    task.Start();
                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("Time of work {0}: ", _system);
                Console.WriteLine(DateTime.Now - timer);
            }
        }

    }
}
