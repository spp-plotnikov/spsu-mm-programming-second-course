using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam_Lab
{
    class User
    {
        private IExamSystem _examSystem;

        public User(IExamSystem examSystem)
        {
            _examSystem = examSystem;
        }

        public bool IsPassed (long studentId, long courseId)
        {
            Task<bool> t = Task.Run(() => _examSystem.Contains(studentId, courseId));
            return t.Result;
        }

        public void Pass (long studentId, long courseId)
        {
            Task.Run(() => _examSystem.Add(studentId, courseId));
        }

        public void Fail (long studentId, long courseId)
        {
            Task.Run(() => _examSystem.Remove(studentId, courseId));
        }
    }
}
