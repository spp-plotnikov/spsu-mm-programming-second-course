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
        List<long[]> _table;
        
        public LockingTable()
        {
            _table = new List<long[]>();
        }

        public void Add(long studentId, long courseId)
        {
            lock (_table)
            {
                long[] temp = new long[2];
                temp[0] = studentId;
                temp[1] = courseId;
                _table.Add(temp);
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_table)
            {
                long[] temp = new long[2];
                temp[0] = studentId;
                temp[1] = courseId;
                _table.Remove(temp);
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (_table)
            {
                long[] temp = new long[2];
                temp[0] = studentId;
                temp[1] = courseId;
                return _table.Contains(temp);
            }
        }

    }
}
