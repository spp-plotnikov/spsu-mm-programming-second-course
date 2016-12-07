using UniversityRequest.HashSets;

namespace UniversityRequest.ExamSystems
{
    public class SmartSystem : IExamSystem
    {

        private StripedHashSet<SystemRecord> _system = new StripedHashSet<SystemRecord>(5297);

        public void Add(long studentId, long courseId)
        {
            _system.Add(new SystemRecord(studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            _system.Remove(new SystemRecord(studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return _system.Contains(new SystemRecord(studentId, courseId));
        }
    }
}