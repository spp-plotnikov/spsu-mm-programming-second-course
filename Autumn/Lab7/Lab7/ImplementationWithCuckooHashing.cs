using System.Collections.Generic;

namespace Lab7
{
    public class ImplementationWithCuckooHashing : IExamSystem
    {
        ConcurrentCuckooHashing _table;
        int _capacity;

        public ImplementationWithCuckooHashing()
        {
            _capacity = 50;
            _table = new ConcurrentCuckooHashing(_capacity);
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
