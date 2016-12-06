using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    class ComplicatedSystem : IExamSystem
    {
        private ConcurrentCuckooHashSet _studentStories;

        public ComplicatedSystem()
        {
            _studentStories = new ConcurrentCuckooHashSet(17);
        }

        public void Add(long studentId, long courseId)
        {
            KeyValuePair<long, long> toAdd = new KeyValuePair<long, long>(studentId, courseId);
            _studentStories.Add(toAdd);
        }

        public void Remove(long studentId, long courseId)
        {
            KeyValuePair<long, long> toRemove = new KeyValuePair<long, long>(studentId, courseId);
            _studentStories.Remove(toRemove);
        }

        public bool Contains(long studentId, long courseId)
        {
            KeyValuePair<long, long> toCheck = new KeyValuePair<long, long>(studentId, courseId);
            return _studentStories.Contains(toCheck);
        }
    }
}
