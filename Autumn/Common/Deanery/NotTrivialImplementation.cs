using System.Collections.Generic;
using System.Threading;

namespace Deanery
{
    class NotTrivialImplementation : IExamSystem
    {
        private SortedList<long, MySortedList<long>> listOfStrudentCredits = new SortedList<long, MySortedList<long>>();
        private MySortedList<long> lockedStudents = new MySortedList<long>();
        private Mutex lockedStudentsMutex = new Mutex(false);
        private Mutex listOfStudentCreditsMutex = new Mutex(false);
        private MySortedList<long> emptyList = new MySortedList<long>();
        
        private bool isStudentInfoAccessible(long studentId)
        {
            return !lockedStudents.Contains(studentId);
        }

        private void TakeStudent(long studentId) // If we need to do smth with student
        {
            lockedStudentsMutex.WaitOne();
            while (lockedStudents.Contains(studentId))
            {
                lockedStudentsMutex.ReleaseMutex();
                lockedStudentsMutex.WaitOne();
            }
            lockedStudents.Add(studentId);
            lockedStudentsMutex.ReleaseMutex();
        }

        private void ReleaseStudent(long studentId) // If we did smth with student
        {
            lockedStudentsMutex.WaitOne();
            lockedStudents.Remove(studentId);
            lockedStudentsMutex.ReleaseMutex();
        }

        public void Add(long studentId, long courseId)
        {
            TakeStudent(studentId);
            // There are 2 options: this credit is first for this student, so we need to lock whole listOfStudentCredits
            // Otherwise usual add of courseId
            listOfStudentCreditsMutex.WaitOne();
            MySortedList<long> tempList;
            bool success = listOfStrudentCredits.TryGetValue(studentId, out tempList);
            if (success)
            {
                listOfStudentCreditsMutex.ReleaseMutex();
                tempList.Add(courseId);
            }
            else
            {
                listOfStrudentCredits.Add(studentId, new MySortedList<long> { courseId });
                listOfStudentCreditsMutex.ReleaseMutex();
            }
            ReleaseStudent(studentId);
        }

        public void Remove(long studentId, long courseId)
        {
            TakeStudent(studentId);
            // There are 2 options: this credit is single for this student, so we need to lock whole listOfStudentCredits
            // Otherwise usual remove of courseId
            listOfStudentCreditsMutex.WaitOne();
            MySortedList<long> tempList;
            bool success = listOfStrudentCredits.TryGetValue(studentId, out tempList);
            if (success && tempList.Count > 1)
            {
                listOfStudentCreditsMutex.ReleaseMutex();
                tempList.Remove(courseId);
            }
            else
            {
                listOfStrudentCredits.Remove(studentId);
                listOfStudentCreditsMutex.ReleaseMutex();
            }
            ReleaseStudent(studentId);
        }

        public bool Contains(long studentId, long courseId)
        {
            TakeStudent(studentId);
            // Just usual Contains
            listOfStudentCreditsMutex.WaitOne();
            MySortedList<long> tempList;
            bool success = listOfStrudentCredits.TryGetValue(studentId, out tempList);
            bool answer;
            listOfStudentCreditsMutex.ReleaseMutex();
            if (success)
            {
                answer = tempList.Contains(courseId);
            }
            else
            {
                answer = false;
            }
            ReleaseStudent(studentId);
            return answer;
        }
    }
}
