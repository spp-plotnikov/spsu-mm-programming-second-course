using System.Collections.Generic;
using System.Threading;

namespace Deanery
{
    class NotTrivialImplementation : IExamSystem
    {
        private SortedList<long, MySortedList<long>> listOfStrudentCredits = new SortedList<long, MySortedList<long>>();
        private MySortedList<long> lockedStudents = new MySortedList<long>();
        private bool lockedStudentsInTouch = true;
        private bool listOfStudentCreditsInTouch = true;
        private MySortedList<long> emptyList = new MySortedList<long>();

        private bool isStudentInfoAccessible(long studentId)
        {
            return !lockedStudents.Contains(studentId);
        }

        public void Add(long studentId, long courseId)
        {
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            while (!isStudentInfoAccessible(studentId))
            {
                lockedStudentsInTouch = true;
                Thread.Sleep(100);
                while (!lockedStudentsInTouch) { }
                lockedStudentsInTouch = false;
            }  // Waiting for student release
            lockedStudents.Add(studentId);
            lockedStudentsInTouch = true;
            // There are 2 options: this credit is first for this student, so we need to lock whole listOfStudentCredits
            // Otherwise usual add of courseId
            long studentIndex = listOfStrudentCredits.IndexOfKey(studentId);
            if (studentIndex >= 0)
            {
                listOfStrudentCredits[studentId].Add(courseId);
            }
            else
            {
                while (!listOfStudentCreditsInTouch) { } // Waiting for locking everything
                listOfStudentCreditsInTouch = false;
                listOfStrudentCredits.Add(studentId, new MySortedList<long> { courseId }); // ?!?!?!?!?!?!?!?!?!?!?!?!?!?
                listOfStudentCreditsInTouch = true;
            }
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            lockedStudents.Remove(studentId);
            lockedStudentsInTouch = true;
        }

        public void Remove(long studentId, long courseId)
        {
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            while (!isStudentInfoAccessible(studentId))
            {
                lockedStudentsInTouch = true;
                Thread.Sleep(100);
                while (!lockedStudentsInTouch) { }
                lockedStudentsInTouch = false;
            }  // Waiting for student release
            lockedStudents.Add(studentId);
            lockedStudentsInTouch = true;
            // There are 2 options: this credit is single for this student, so we need to lock whole listOfStudentCredits
            // Otherwise usual remove of courseId
            int studentIndex = listOfStrudentCredits.IndexOfKey(studentId);
            if (studentIndex >= 0)
            {
                listOfStrudentCredits[studentId].Remove(courseId);
            }
            else
            {
                while (!listOfStudentCreditsInTouch) { } // Waiting for locking everything
                listOfStudentCreditsInTouch = false;
                listOfStrudentCredits.RemoveAt(studentIndex); // ?!?!?!?!?!?!?!?!?!?!?!?!?!?
                listOfStudentCreditsInTouch = true;
            }
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            lockedStudents.Remove(studentId);
            lockedStudentsInTouch = true;
        }

        public bool Contains(long studentId, long courseId)
        {
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            while (!isStudentInfoAccessible(studentId)) { }  // Waiting for student release
            lockedStudents.Add(studentId);
            lockedStudentsInTouch = true;
            // Just usual Contains
            int studentIndex = listOfStrudentCredits.IndexOfKey(studentId);
            bool answer;
            if (studentIndex >= 0)
            {
                answer = listOfStrudentCredits[studentId].Contains(courseId);
            }
            else
            {
                answer = false;
            }
            while (!lockedStudentsInTouch) { } // Waiting for access list of locked students
            lockedStudentsInTouch = false;
            lockedStudents.Remove(studentId);
            lockedStudentsInTouch = true;
            return answer;
        }
    }
}
