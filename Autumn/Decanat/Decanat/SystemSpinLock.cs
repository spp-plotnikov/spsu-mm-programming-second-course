using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    class SystemSpinLock: IExamSystem
    {
        const int numStudents = 3000;
        const int numTeachers = 200;
        private SpinLock _sl = new SpinLock();
        private bool _lockTaken = false;

        static bool[,,] exam = new bool[numStudents, numTeachers, 2];



        public void Add(long studentID, long courseID, job job)
        {
            while (_lockTaken)
            {
            }
            bool _thisLockTaken = false;
            _sl.Enter(ref _thisLockTaken);
            _lockTaken = true;
            if (job == job.Student)                 //Потому, что последние 4 цифры studentId - порядковый номер студента.
                exam[studentID % 10000, courseID, 1] = true;
            else
                exam[studentID % 10000, courseID, 0] = true;        //Отметка учителя, как подтверждение слов студента
            if (_lockTaken)
            {
                _lockTaken = false;
                _sl.Exit();
            }
        }

        public void Remove(long studentID, long courseID, job job)
        {
            while (_lockTaken)
            {
            }
            bool _thisLockTaken = false;
            _sl.Enter(ref _thisLockTaken);
            _lockTaken = true;
            if (job == job.Student)
                exam[studentID % 10000, courseID, 1] = false;
            else
                exam[studentID % 10000, courseID, 0] = false;
            if (_lockTaken)
            {
                _lockTaken = false;
                _sl.Exit();
            }
        }

        public bool Contains(long studentID, long courseID)
        {
            try
            {
                while (_lockTaken)
                {
                }
                bool _thisLockTaken = false;
                _sl.Enter(ref _thisLockTaken);
                _lockTaken = true;
                return exam[studentID % 10000, courseID, 0];
            }
            finally
            {
                if (_lockTaken)
                {
                    _lockTaken = false;
                    _sl.Exit();
                }
            }
        }
    }
}
