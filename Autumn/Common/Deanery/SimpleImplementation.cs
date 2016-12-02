using System.Collections.Generic;
using System.Threading;

namespace Deanery
{
    class SimpleImplementation : IExamSystem
    {
        private HashSet<Credit> credits = new HashSet<Credit>();
        private Mutex hashSetMutex = new Mutex(false);
        public void Add(long studentId, long courseId)
        {
            hashSetMutex.WaitOne();
            credits.Add(new Credit(studentId, courseId));
            hashSetMutex.ReleaseMutex();
        }

        public void Remove(long studentId, long courseId)
        {
            hashSetMutex.WaitOne();
            credits.Remove(new Credit(studentId, courseId));
            hashSetMutex.ReleaseMutex();
        }

        public bool Contains(long studentId, long courseId)
        {
            hashSetMutex.WaitOne();
            bool answer = credits.Contains(new Credit(studentId, courseId));
            hashSetMutex.ReleaseMutex();
            return answer;
        }
    }
}
