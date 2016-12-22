using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lab7
{
    public class SimpleImplementation : IExamSystem
    {
        HashSet<KeyValuePair<long, long>> _table;
        object _locker;
        bool _isContain;

        public SimpleImplementation()
        {
            _table = new HashSet<KeyValuePair<long, long>>();
            _locker = new object();
        }

        public void Add(long studentId, long courseId)
        {
            lock (_locker)
            {
                _table.Add(new KeyValuePair<long, long>(studentId, courseId));
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (_locker)
            {
                _isContain = _table.Contains(new KeyValuePair<long, long>(studentId, courseId));
            }
            return _isContain;
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_locker)
            {
                _table.Remove(new KeyValuePair<long, long>(studentId, courseId));
            }
        }
    }
}
