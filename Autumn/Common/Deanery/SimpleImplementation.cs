using System.Collections.Generic;

namespace Deanery
{
    class SimpleImplementation : IExamSystem
    {
        private HashSet<Credit> credits = new HashSet<Credit>();
        private bool inTouch = true;
        public void Add(long studentId, long courseId)
        {
            while (!inTouch) { }
            inTouch = false;
            credits.Add(new Credit(studentId, courseId));
            inTouch = true;
        }

        public void Remove(long studentId, long courseId)
        {
            while (!inTouch) { }
            inTouch = false;
            credits.Remove(new Credit(studentId, courseId));
            inTouch = true;
        }

        public bool Contains(long studentId, long courseId)
        {
            while (!inTouch) { }
            inTouch = false;
            bool answer = credits.Contains(new Credit(studentId, courseId));
            inTouch = true;
            return answer;
        }
    }
}
