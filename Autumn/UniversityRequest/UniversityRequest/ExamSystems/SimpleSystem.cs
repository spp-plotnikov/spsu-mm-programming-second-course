using System;
using System.Collections.Generic;

namespace UniversityRequest.ExamSystems
{
    public class SimpleSystem : IExamSystem
    {
        private HashSet<SystemRecord> _system = new HashSet<SystemRecord>();
        private object _lock = new object();

        public void Add(long studentId, long courseId)
        {
            lock (_lock)
            {
                _system.Add(new SystemRecord(studentId, courseId));
                //.WriteLine("add");
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_lock)
            {
                _system.Remove(new SystemRecord(studentId, courseId));
                //Console.WriteLine("remove");
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (_lock)
            {
                //Console.WriteLine("Contains");
                return _system.Contains(new SystemRecord(studentId, courseId));
            }
        }
    }
}