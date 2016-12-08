using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ExamStorage
{
    class SlowStorage : IExamSystem
    {
        private Dictionary<long, SortedSet<long>> table = new Dictionary<long, SortedSet<long>>();
        private Mutex lockTable = new Mutex();

        public void Add(long studentId, long courseId)
        {
            // entry critical section
            lockTable.WaitOne();

            // student already exist 
            if (table.ContainsKey(studentId) && !table[studentId].Contains(courseId))
                table[studentId].Add(courseId);
            else if (!table.ContainsKey(studentId))
                table.Add(studentId, new SortedSet<long> { courseId });

            // leave crtial section
            lockTable.ReleaseMutex();
        }

        public void Remove(long studentId, long courseId)
        {
            lockTable.WaitOne();

            // check exist and remove 
            if (table.ContainsKey(studentId) && table[studentId].Contains(courseId))
                table[studentId].Remove(courseId);

            lockTable.ReleaseMutex();
        }

        public bool Contains(long studentId, long courseId)
        {
            lockTable.WaitOne();

            bool exist = (table.ContainsKey(studentId) && table[studentId].Contains(courseId));

            lockTable.ReleaseMutex();

            return exist;
        }

        /*public void Print()
        {
            lockTable.WaitOne();
            foreach (KeyValuePair<long, SortedSet<long>> kvp in table)
                foreach (long kvp1 in kvp.Value)
                    Console.WriteLine("Key = " + kvp.Key + ", Value = " + kvp1 + "");
            lockTable.ReleaseMutex();
        }*/
    }
}
