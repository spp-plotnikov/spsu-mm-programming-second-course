using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    class LockSystem : IExamSystem
    {
        private List<KeyValuePair<long, long>> _studentsStories;

        public LockSystem()
        {
            _studentsStories = new List<KeyValuePair<long, long>>();
        }

        public void Add(long studentId, long courseId)
        {
            Monitor.Enter(_studentsStories);
            KeyValuePair<long, long> toInsert = new KeyValuePair<long, long>(studentId, courseId);
            _studentsStories.Add(toInsert);
            Monitor.Exit(_studentsStories);
        }
        public void Remove(long studentId, long courseId)
        {
            Monitor.Enter(_studentsStories);
            KeyValuePair<long, long> toDelete = new KeyValuePair<long, long>(studentId, courseId);
            _studentsStories.Remove(toDelete);
            Monitor.Exit(_studentsStories);
        }

        public bool Contains(long studentId, long courseId)
        {
            bool check;
            Monitor.Enter(_studentsStories);
            KeyValuePair<long, long> toCheck = new KeyValuePair<long, long>(studentId, courseId);
            check = _studentsStories.Contains(toCheck);
            Monitor.Exit(_studentsStories);
            return check;
        }
    }
}
