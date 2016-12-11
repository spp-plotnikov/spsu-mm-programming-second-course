using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ForUniversity
{
    class LockingPartOfTable : IExamSystem
    {
        private Dictionary<long, List<long>> _table = new Dictionary<long, List<long>>();
        private List<long> _studetsToChange = new List<long>();
        private Mutex _lock = new Mutex();

        private void LockStudent (long studentId)
        {
            while (true)
            {
                lock (_studetsToChange)
                {
                    if (_studetsToChange.Contains(studentId))
                    {
                        Monitor.Wait(_studetsToChange);
                    }
                    else
                    {
                        _studetsToChange.Add(studentId);
                        Monitor.PulseAll(_studetsToChange);
                        return;
                    }
                }
            }
        }

        private void UnlockStudent (long studentId)
        {
            lock (_studetsToChange)
            {
                _studetsToChange.Remove(studentId);
                Monitor.PulseAll(_studetsToChange);
            }
        }


        public void Add(long studentId, long courseId)
        {
            LockStudent(studentId);

            _lock.WaitOne();

            if (_table.ContainsKey(studentId))
            {
                _lock.ReleaseMutex();
                if (!_table[studentId].Contains(courseId))
                {
                    _table[studentId].Add(courseId);
                }
                UnlockStudent(studentId);
                return;
            }

            _table.Add(studentId, new List<long> { courseId });
            UnlockStudent(studentId);
            _lock.ReleaseMutex();
        }

        public void Remove(long studentId, long courseId)
        {
            LockStudent(studentId);
            

            if (_table.ContainsKey(studentId))
            {
                _table[studentId].Remove(courseId);
                UnlockStudent(studentId);
                return;
            }
            UnlockStudent(studentId);
        }

        public bool Contains(long studentId, long courseId)
        {
            LockStudent(studentId);
            if (_table.ContainsKey(studentId))
            {
                bool res = _table[studentId].Contains(courseId);
                UnlockStudent(studentId);
                return res;
            }
            UnlockStudent(studentId);
            return false;
        }
    }
}
