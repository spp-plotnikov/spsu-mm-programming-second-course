using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    class Teacher
    {
        private IExamSystem _decanatSystem;
        private Thread _myTeacher;
        private Random _examGen;

        public Teacher(IExamSystem newSystem)
        {
            _decanatSystem = newSystem;
            _examGen = new Random();
            _myTeacher = new Thread(() => Run());
            _myTeacher.Start();
        }

        /// <summary>
        /// The output for all operations was commented for clearer picture
        /// You may uncomment them in order to see the result of every operation
        /// </summary>
        public void Run()
        {
            int numOfStudents = 1000; 
            int numOfCourses = 100;
            // the whole amount of calls: 1000 = 100%

            // add calls: 90 = 10%
            for (int i = 0; i < 90; i++)
            {
                KeyValuePair<long, long> toInsert = MakeExamForStudent(numOfStudents, numOfCourses);
                _decanatSystem.Add(toInsert.Key, toInsert.Value);
                //Console.WriteLine("Student {0} passed {1} exam", toInsert.Key, toInsert.Value);
            }

            // remove calls 10 = 1%
            for (int i = 0; i < 10; i++)
            {
                KeyValuePair<long, long> toRemove = MakeExamForStudent(numOfStudents, numOfCourses);
                _decanatSystem.Remove(toRemove.Key, toRemove.Value);
              //  Console.WriteLine("Student {0} failed {1} exam", toRemove.Key, toRemove.Value);
            }

            // contains calls 900 = 90%
            for (int i = 0; i < 900; i++)
            {
                KeyValuePair<long, long> toCheck = MakeExamForStudent(numOfStudents, numOfCourses);
                bool ans = _decanatSystem.Contains(toCheck.Key, toCheck.Value);

                if (ans)
                {
                   // Console.WriteLine("Student {0} attended {1} exam", toCheck.Key, toCheck.Value);
                }
                else
                {
                   // Console.WriteLine("Student {0} didn't attend {1} exam", toCheck.Key, toCheck.Value);
                }
            }

        }

        private KeyValuePair<long, long> MakeExamForStudent(int numOfStudents, int numOfCourses)
        {
            int studentId = _examGen.Next(numOfStudents);
            int courseID = _examGen.Next(numOfCourses);
            return new KeyValuePair<long, long>(studentId, courseID);
        }

        public void Finish()
        {
            _myTeacher.Join();
        }
    }
}
