using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem
{
    class ExamSystemFirst : IExamSystem
    {
        private HashSet<Tuple<long, long>> hashTable;
        private bool isPrinting = false;

        public ExamSystemFirst()
        {
            hashTable = new HashSet<Tuple<long, long>>();
        }

        public void Add(long studentId, long courseId)
        {
            lock(hashTable)
            {
                hashTable.Add(new Tuple<long, long>(studentId, courseId));
                if (isPrinting)
                {
                    Console.WriteLine("System First added (" + studentId + " : " + courseId + ") in table");
                }
            }
        }

        public void Remove(long studentId, long courseId)
        {
            lock (hashTable)
            {
                hashTable.Remove(new Tuple<long, long>(studentId, courseId));
                if (isPrinting)
                {
                    Console.WriteLine("System First remove (" + studentId + " : " + courseId + ") out of table");
                }
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            lock (hashTable)
            {
                bool res = hashTable.Contains(new Tuple<long, long>(studentId, courseId));
                if (isPrinting)
                {
                    Console.WriteLine("System First check is (" + studentId + " : " + courseId + ") constains in table, answer = " + res);
                }
                return res;
            }
        }
    }
}
