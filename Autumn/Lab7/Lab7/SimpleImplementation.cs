using System.Collections.Generic;

namespace Lab7
{
    public class SimpleImplementation : IExamSystem
    {
        private HashSet<KeyValuePair<long, long>> _table;

        public SimpleImplementation()
        {
            _table = new HashSet<KeyValuePair<long, long>>();
        }

        public void Add(long studentId, long courseId)
        {
            _table.Add(new KeyValuePair<long, long>(studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return _table.Contains(new KeyValuePair<long, long>(studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            _table.Remove(new KeyValuePair<long, long>(studentId, courseId));
        }
    }
}
