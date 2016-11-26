using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Org1 : IExamSystem
    {
        List<CreditItem>[] hashTable = null;
        int tableSize = 10;
        int listSize = 5;
        Mutex mut = new Mutex();

        public Org1()
        {            
            CreateTable();            
        }
        
        void CreateTable()
        {
            hashTable = new List<CreditItem>[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                hashTable[i] = new List<CreditItem>();
            }
        }
       
        void Resize()//блок всей таблицы
        {            
            Console.WriteLine("Resize");
            Thread.Sleep(2000);
            mut.WaitOne();
            List<CreditItem>[] buffer = hashTable;
            tableSize *= 2;
            CreateTable();

            foreach (var ht in buffer)
            {
                foreach(var el in ht)
                {
                    hashTable[el.hash % tableSize].Add(el);
                }
            }
            mut.ReleaseMutex();        
        }

        //блок ячейки для всех этих методов
        public void Add(long studentId, long courseId)
        {
            CreditItem el = new CreditItem(studentId, courseId);
            int delta = Convert.ToInt32(el.hash % tableSize);
            el.passed = true;
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
            CreditItem el = new CreditItem(studentId, courseId);
            int delta = Convert.ToInt32(el.hash % tableSize);
            mut.WaitOne();
            foreach (var item in hashTable[delta])
            {
                if(item.studentID == el.studentID)
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
            CreditItem el = new CreditItem(studentId, courseId);
            int delta = Convert.ToInt32(el.hash % tableSize);
            mut.WaitOne();
            int count = 0;
            foreach (var item in hashTable[delta])
            {
                if (item.studentID == el.studentID)
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
