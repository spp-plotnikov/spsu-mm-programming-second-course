using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ForUniversity
{
    class LockingTable : IExamSystem
    {
        List<KeyValuePair<long, long>> _table;
        
        public LockingTable()
        {
            _table = new List<KeyValuePair<long, long>>();
        }

        public void Add(long studentId, long courseId)
        {
            lock (_table)
            {
                KeyValuePair<long, long> temp = new KeyValuePair<long, long>(studentId, courseId);               
                _table.Add(temp);
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_table)
            {
                KeyValuePair<long, long> temp = new KeyValuePair<long, long>(studentId, courseId);
                _table.Remove(temp);
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (_table)
            {
                KeyValuePair<long, long> temp = new KeyValuePair<long, long>(studentId, courseId);
                return _table.Contains(temp);
            }
        }

    }
}
