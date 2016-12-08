using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace ExamSystem
{
    class Checker
    {
        private readonly int numTeachers = 5; 
        private readonly int numExams = 1000;
        private List<Tuple<long, long>> exams = new List<Tuple<long, long>>();
        private Random rnd = new Random();

        public Checker()
        {
            for (int i = 0; i < numExams; i++)
            {
                exams.Add(new Tuple<long, long>(rnd.Next(), rnd.Next()));
            }
        }

        public String GetTime(IExamSystem system)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //-----------------------------------------


            List<Teacher> teachers = new List<Teacher>();
            for (int i = 0; i < numTeachers; i++)
            {
                Teacher teacher = new Teacher(i, exams, system);
                teachers.Add(teacher);
            }

            for (int i = 0; i < numTeachers; i++)
            {
                teachers[i].ThreadJoin();
            }
            teachers.Clear();

            //-----------------------------------------
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return elapsedTime;
        }
    }
}
