using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem
{
    class ExamSystemSecond : IExamSystem
    {
        private ConcurrentHashSet exams;
        private bool isPrinting = false;

        public ExamSystemSecond(int numLocks)
        {
            exams = new ConcurrentHashSet(25, numLocks);
        }

        public void Add(long studentId, long courseId)
        {
            exams.Add(new Tuple<long, long>(studentId, courseId));
            //if (isPrinting)
            //{
            //    Console.WriteLine("System Second added (" + studentId + " : " + courseId + ") in table");
            //}
        }

        public void Remove(long studentId, long courseId)
        {
            exams.Remove(new Tuple<long, long>(studentId, courseId));
            //if (isPrinting)
            //{
            //    Console.WriteLine("System Second remove (" + studentId + " : " + courseId + ") out of table");
            //}
        }

        public bool Contains(long studentId, long courseId)
        {
            bool res = exams.Contains(new Tuple<long, long>(studentId, courseId));
            //if (isPrinting)
            //{
            //    Console.WriteLine("System Second check is (" + studentId + " : " + courseId + ") constains in table, answer = " + res);
            //}
            return res;
        }
    }
}
