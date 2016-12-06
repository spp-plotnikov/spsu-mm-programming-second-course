using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    class Timer
    {
        private IExamSystem _mySystem;
        private string _name;

        public Timer(IExamSystem newSystem, string name)
        {
            _mySystem = newSystem;
            _name = name;
        }

        public void CountTime()
        {
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            int numOfTeachers = 5;
            Teacher[] teachers = new Teacher[numOfTeachers];

            for (int i = 0; i < numOfTeachers; i++)
            {
                teachers[i] = new Teacher(_mySystem);
            }

            for (int i = 0; i < numOfTeachers; i++)
            {
                teachers[i].Finish();
            }

            timer.Stop();
            Console.WriteLine ("The system " + _name +  " was executing for " + (timer.ElapsedMilliseconds).ToString() + " milliseconds!");
        }
    }
}
