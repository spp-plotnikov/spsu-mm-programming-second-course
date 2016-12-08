using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ExamStorage
{
    class FastStorage : IExamSystem
    {
        private Dictionary<long, SortedSet<long>> table = new Dictionary<long, SortedSet<long>>();
        private SortedSet<long> lockedRows = new SortedSet<long>(); // set of current locked rows 
        private Mutex lockTable = new Mutex();

        // using monitor for concurency access
        // when row already blocked other threads wait  for release 
        private void lockRow(long rowId)
        {
            lock (lockedRows)
            {
                if (lockedRows.Contains(rowId))
                {
                    Monitor.Wait(lockedRows);
                }
                else
                {
                    lockedRows.Add(rowId);
                    Monitor.Pulse(lockedRows);
                }
            }
        }

        // after end of work with row release lock
        private void unlockRow(long rowId)
        {
            lock (lockedRows)
            {
                lockedRows.Remove(rowId);
                Monitor.Pulse(lockedRows);
            }
        }

        public void Add(long studentId, long courseId)
        {
            // block item 
            lockRow(studentId);

            // lock all table 
            lockTable.WaitOne();

            // can adding?
            if (table.ContainsKey(studentId) && !table[studentId].Contains(courseId))
            {
                // unlock table 
                lockTable.ReleaseMutex();

                // and working with one branch 
                table[studentId].Add(courseId);

                // unblock cur item
                unlockRow(studentId);

                return;
            }

            // if branch not exist, adding 
            if (!table.ContainsKey(studentId))
                table.Add(studentId, new SortedSet<long> { courseId });

            // and unlocking table
            lockTable.ReleaseMutex();

            // unlock item
            unlockRow(studentId);
        }

        public void Remove(long studentId, long courseId)
        {
            lockRow(studentId);

            lockTable.WaitOne();

            if (table.ContainsKey(studentId) && table[studentId].Contains(courseId))
            {
                lockTable.ReleaseMutex();
                table[studentId].Remove(courseId);
                unlockRow(studentId);
                return;
            }

            lockTable.ReleaseMutex();

            unlockRow(studentId);
        }

        public bool Contains(long studentId, long courseId)
        {
            SortedSet<long> tmp = new SortedSet<long>();
            lockRow(studentId);

            // lock table and take needed branch
            lockTable.WaitOne();

            if (table.ContainsKey(studentId))
                tmp = table[studentId];

            lockTable.ReleaseMutex();

            bool exist = tmp.Contains(courseId);

            unlockRow(studentId);
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
