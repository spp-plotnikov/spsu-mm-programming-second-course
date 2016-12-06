using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExamSystems
{
    class EasyOrganisation : IExamSystem
    {
        List<SystemItem>[] hashTable = null;
        int tableSize = 8;
        int listSize = 4;
        Mutex mut = new Mutex();

        public EasyOrganisation()
        {
            CreateTable();
        }

        void CreateTable()
        {
            hashTable = new List<SystemItem>[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                hashTable[i] = new List<SystemItem>();
            }
        }

        void Resize()//блок всей таблицы
        {
            //Console.WriteLine("Resize");
            mut.WaitOne();
            List<SystemItem>[] buffer = hashTable;
            tableSize *= 2;
            listSize *= 2;
            CreateTable();

            foreach (var ht in buffer)
            {
                foreach (var el in ht)
                {
                    hashTable[el.Hash % tableSize].Add(el);
                }
            }
            mut.ReleaseMutex();
        }

        //блок ячейки для всех этих методов
        public void Add(long studentId, long courseId)
        {
            SystemItem el = new SystemItem(studentId, courseId);
            int delta = Convert.ToInt32(el.Hash % tableSize);
            mut.WaitOne();
            hashTable[delta].Add(el);
            mut.ReleaseMutex();
            if (hashTable[delta].Count() > listSize)
            {
                Resize();
            }

        }

        public bool Contains(long studentId, long courseId)
        {
            SystemItem el = new SystemItem(studentId, courseId);
            int delta = Convert.ToInt32(el.Hash % tableSize);
            mut.WaitOne();
            foreach (var item in hashTable[delta])
            {
                if (item.StudentID == el.StudentID)
                {
                    mut.ReleaseMutex();
                    return true;
                }
            }
            mut.ReleaseMutex();
            return false;
        }

        public void Remove(long studentId, long courseId)
        {
            SystemItem el = new SystemItem(studentId, courseId);
            int delta = Convert.ToInt32(el.Hash % tableSize);
            mut.WaitOne();
            int count = 0;
            foreach (var item in hashTable[delta])
            {
                if (item.StudentID == el.StudentID)
                {
                    hashTable[delta].RemoveAt(count);
                    mut.ReleaseMutex();
                    return;
                }
                count++;
            }
            mut.ReleaseMutex();
        }
    }
}
