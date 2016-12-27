using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7
{
    class SlowSystem : IExamSystem
    {
        private HashSet<KeyValuePair<long, long>> table = new HashSet<KeyValuePair<long, long>>();
        private object threadLock = new object();

        public void Add(long studentId, long courseId)
        {
            lock(threadLock)
            {
                table.Add(new KeyValuePair<long, long>(studentId, courseId));
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock(threadLock)
            {
                table.Remove(new KeyValuePair<long, long>(studentId, courseId));
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock(threadLock)
            {
                return table.Contains(new KeyValuePair<long, long>(studentId, courseId));
            }
        }
    }
}
