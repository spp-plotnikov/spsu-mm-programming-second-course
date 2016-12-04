using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystems
{
    class BucketOrganisation : IExamSystem
    {
        ListItem total;
        Bucket[] buckets;
        int tableSize = 8;
        int listSize = 4;

        public BucketOrganisation()
        {
            total = new Bucket(0);
            buckets = new Bucket[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                buckets[i] = new Bucket(i);
            }
            buckets[0].next = total;
        }

        private void AddBucket(int number)
        {
            ListItem preNode = total;
            ListItem node = preNode.next;
            Bucket newBuck = new Bucket(number);

            while (node != null)
            {
                if (node.bin.CompareTo(newBuck.bin) < 0)
                {
                    preNode = node;
                    node = node.next;
                }
                else
                {
                    break;
                }
            }

            if (preNode.next == null)
            {
                preNode.next = newBuck;
                buckets[number].next = preNode.next;
            }
            else
            {
                preNode.next = newBuck;
                buckets[number].next = newBuck;
                newBuck.next = node;

            }
        }

        private bool Validate(ListItem pred, ListItem curr, int n)
        {
            ListItem node = buckets[n];
            while (node.bin.CompareTo(pred.bin) <= 0)
            {
                if (node == pred)
                    return pred.next == curr;
                node = node.next;
            }
            return false;
        }

        public void Add(long studentId, long courseId)
        {
            CreditItem curSt = new CreditItem(studentId, courseId);
            int delta = curSt.hash % tableSize;
            if (buckets[delta].next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                int len = 0;
                ListItem preNode = buckets[delta].next;
                ListItem node = preNode.next;
                while (node != null && node.bin.CompareTo(curSt.bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    len++;
                    preNode = node;
                    node = node.next;
                }
                if (node == null)
                {
                    preNode.mutex.WaitOne();
                    preNode.next = curSt;
                    preNode.mutex.ReleaseMutex();
                    return;

                }
                if (preNode.isBucket && node.isBucket)
                {
                    preNode.mutex.WaitOne();
                    curSt.next = node;
                    preNode.next = curSt;
                    preNode.mutex.ReleaseMutex();
                    return;
                }
                if (len > listSize)
                {
                    Resize();
                    Add(curSt.studentID, curSt.courseID);
                    return;
                }

                preNode.mutex.WaitOne();
                node.mutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    if (node.bin.CompareTo(curSt.bin) == 0) return;
                    else
                    {
                        curSt.next = node;
                        preNode.next = curSt;
                        return;
                    }
                }
                finally
                {
                    preNode.mutex.ReleaseMutex();
                    node.mutex.ReleaseMutex();
                }
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            CreditItem curSt = new CreditItem(studentId, courseId);
            int delta = curSt.hash % tableSize;
            Console.WriteLine("{0} {1}", curSt.hash, delta);

            if (buckets[delta].next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                ListItem preNode = buckets[delta].next;
                ListItem node = preNode.next;
                while (node != null && node.bin.CompareTo(curSt.bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    preNode = node;
                    node = node.next;
                }

                if (node == null || preNode.isBucket && node.isBucket)
                {
                    return false;
                }

                preNode.mutex.WaitOne();
                node.mutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    return node.bin.Equals(curSt.bin);
                }
                finally
                {
                    preNode.mutex.ReleaseMutex();
                    node.mutex.ReleaseMutex();
                }
            }
        }

        public void Remove(long studentId, long courseId)
        {
            CreditItem curSt = new CreditItem(studentId, courseId);
            int delta = curSt.hash % tableSize;
            if (buckets[delta].next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                ListItem preNode = buckets[delta].next;
                ListItem node = preNode.next;
                while (node != null && node.bin.CompareTo(curSt.bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    preNode = node;
                    node = node.next;
                }
                if (node == null || preNode.isBucket && node.isBucket)
                {
                    return;
                }

                preNode.mutex.WaitOne();
                node.mutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    if (node.bin.CompareTo(curSt.bin) == 0)
                    {
                        preNode.next = node.next;
                    }
                    return;
                }
                finally
                {
                    preNode.mutex.ReleaseMutex();
                    node.mutex.ReleaseMutex();
                }
            }
        }

        public void Print()
        {
            for (var t = total; t != null; t = t.next)
            {
                Console.Write("{0}: {1} -> ", t.isBucket, t.bin);
            }
            Console.WriteLine();
        }

        private void Resize()
        {
            Console.WriteLine("Resize.");
            Thread.Sleep(1000);
            int newSize = tableSize * 2;
            Array.Resize(ref buckets, newSize);
            for (int i = tableSize; i < newSize; i++)
            {
                buckets[i] = new Bucket(i);
            }
            tableSize = newSize;
            listSize = tableSize / 2;
        }
    }
}
