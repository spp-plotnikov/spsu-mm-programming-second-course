using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Lab
{
    class LockListExamSystem : IExamSystem
    {
        private List<KeyValuePair<long, long>> _students = new List<KeyValuePair<long, long>>();

        public void Add(long studentId, long courseId)
        {
            lock(_students)
            {
                var st = new KeyValuePair<long, long>(studentId, courseId);
                if (!_students.Contains(st))
                    _students.Add(new KeyValuePair<long, long>(studentId, courseId));
            }

        }

        public bool Contains(long studentId, long courseId)
        {
            bool res;
            lock (_students)
            {
                res = _students.Contains(new KeyValuePair<long, long>(studentId, courseId));
            }
            return res;
        }

        public void Remove(long studentId, long courseId)
        {
            lock (_students)
            {
                _students.Remove(new KeyValuePair<long, long>(studentId, courseId));
            }
        }
    }
}
