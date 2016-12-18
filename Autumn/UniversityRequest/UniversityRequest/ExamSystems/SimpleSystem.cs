using System.Collections.Generic;

namespace UniversityRequest.ExamSystems
{
    public class SimpleSystem : IExamSystem
    {
        private readonly HashSet<SystemRecord> _system = new HashSet<SystemRecord>();
        private readonly object _lock = new object();

        public void Add(long studentId, long courseId)
        {
            lock (_lock)
            {
                _system.Add(new SystemRecord(studentId, courseId));
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_lock)
            {
                _system.Remove(new SystemRecord(studentId, courseId));
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (_lock)
            {
                return _system.Contains(new SystemRecord(studentId, courseId));
            }
        }
    }
}