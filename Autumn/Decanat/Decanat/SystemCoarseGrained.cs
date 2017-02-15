using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    class SystemCoarseGrained: IExamSystem
    {
        const int numStudents = 3000;
        const int numTeachers = 200;
        private object _lockTaken = new object();

        static bool[,,] exam = new bool[numStudents, numTeachers, 2];



        public void Add(long studentID, long courseID, job job)
        {
            try
            {
                Monitor.Enter(_lockTaken);
                if (job == job.Student)                 //Потому, что последние 4 цифры studentId - порядковый номер студента.
                    exam[studentID % 10000, courseID, 1] = true;
                else
                    exam[studentID % 10000, courseID, 0] = true;        //Отметка учителя, как подтверждение слов студента
            }
            finally
            {
                Monitor.Exit(_lockTaken);
            }

        }

        public void Remove(long studentID, long courseID, job job)
        {

            try
            {
                Monitor.Enter(_lockTaken);
                if (job == job.Student)
                    exam[studentID % 10000, courseID, 1] = false;
                else
                    exam[studentID % 10000, courseID, 0] = false;
            }
            finally
            {
                Monitor.Exit(_lockTaken);
            }
        }

        public bool Contains(long studentID, long courseID)
        {
            try {
                Monitor.Enter(_lockTaken);
                return exam[studentID % 10000, courseID, 0];
            }
            finally
            {
                Monitor.Exit(_lockTaken);
            }
        }
    }
}
