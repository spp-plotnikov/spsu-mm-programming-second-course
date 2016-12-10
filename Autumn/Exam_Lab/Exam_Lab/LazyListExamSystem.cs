using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Lab
{
    class LazyListExamSystem : IExamSystem
    {
        private LazyList<KeyValuePair<long, long>> _students = new LazyList<KeyValuePair<long, long>>();

        public void Add(long studentId, long courseId)
        {
            _students.Add(new KeyValuePair<long, long>(studentId,courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return _students.Contains(new KeyValuePair<long, long>(studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            _students.Remove(new KeyValuePair<long, long>(studentId, courseId));
        }
    }
}
