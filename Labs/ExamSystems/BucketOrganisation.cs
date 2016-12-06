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
            buckets[0].Next = total;
        }

        private void AddBucket(int number)
        {
            ListItem preNode = total;
            ListItem node = preNode.Next;
            Bucket newBuck = new Bucket(number);

            while (node != null)
            {
                if (node.Bin.CompareTo(newBuck.Bin) < 0)
                {
                    preNode = node;
                    node = node.Next;
                }
                else
                {
                    break;
                }
            }

            if (preNode.Next == null)
            {
                preNode.Next = newBuck;
                buckets[number].Next = preNode.Next;
            }
            else
            {
                preNode.Next = newBuck;
                buckets[number].Next = newBuck;
                newBuck.Next = node;

            }
        }

        private bool Validate(ListItem pred, ListItem curr, int n)
        {
            ListItem node = buckets[n];
            while (node.Bin.CompareTo(pred.Bin) <= 0)
            {
                if (node == pred)
                    return pred.Next == curr;
                node = node.Next;
            }
            return false;
        }

        public void Add(long studentId, long courseId)
        {
            SystemItem curSt = new SystemItem(studentId, courseId);
            int delta = curSt.Hash % tableSize;
            if (buckets[delta].Next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                int len = 0;
                ListItem preNode = buckets[delta].Next;
                ListItem node = preNode.Next;
                while (node != null && node.Bin.CompareTo(curSt.Bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    len++;
                    preNode = node;
                    node = node.Next;
                }
                if (node == null)
                {
                    preNode.BucketMutex.WaitOne();
                    preNode.Next = curSt;
                    preNode.BucketMutex.ReleaseMutex();
                    return;

                }
                if (preNode.IsBucket && node.IsBucket)
                {
                    preNode.BucketMutex.WaitOne();
                    curSt.Next = node;
                    preNode.Next = curSt;
                    preNode.BucketMutex.ReleaseMutex();
                    return;
                }
                if (len > listSize)
                {
                    Resize();
                    Add(curSt.StudentID, curSt.CourseID);
                    return;
                }

                preNode.BucketMutex.WaitOne();
                node.BucketMutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    if (node.Bin.CompareTo(curSt.Bin) == 0) return;
                    else
                    {
                        curSt.Next = node;
                        preNode.Next = curSt;
                        return;
                    }
                }
                finally
                {
                    preNode.BucketMutex.ReleaseMutex();
                    node.BucketMutex.ReleaseMutex();
                }
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            SystemItem curSt = new SystemItem(studentId, courseId);
            int delta = curSt.Hash % tableSize;
           // Console.WriteLine("{0} {1}", curSt.Hash, delta);

            if (buckets[delta].Next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                ListItem preNode = buckets[delta].Next;
                ListItem node = preNode.Next;
                while (node != null && node.Bin.CompareTo(curSt.Bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    preNode = node;
                    node = node.Next;
                }

                if (node == null || preNode.IsBucket && node.IsBucket)
                {
                    return false;
                }

                preNode.BucketMutex.WaitOne();
                node.BucketMutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    return node.Bin.Equals(curSt.Bin);
                }
                finally
                {
                    preNode.BucketMutex.ReleaseMutex();
                    node.BucketMutex.ReleaseMutex();
                }
            }
        }

        public void Remove(long studentId, long courseId)
        {
            SystemItem curSt = new SystemItem(studentId, courseId);
            int delta = curSt.Hash % tableSize;
            if (buckets[delta].Next == null)
            {
                AddBucket(delta);
            }

            while (true)
            {
                ListItem preNode = buckets[delta].Next;
                ListItem node = preNode.Next;
                while (node != null && node.Bin.CompareTo(curSt.Bin) < 0) // считаем, что карзины не удаляются никогда
                {
                    preNode = node;
                    node = node.Next;
                }
                if (node == null || preNode.IsBucket && node.IsBucket)
                {
                    return;
                }

                preNode.BucketMutex.WaitOne();
                node.BucketMutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, delta)) continue;
                    if (node.Bin.CompareTo(curSt.Bin) == 0)
                    {
                        preNode.Next = node.Next;
                    }
                    return;
                }
                finally
                {
                    preNode.BucketMutex.ReleaseMutex();
                    node.BucketMutex.ReleaseMutex();
                }
            }
        }

        public void Print()
        {
            for (var t = total; t != null; t = t.Next)
            {
                Console.Write("{0}: {1} -> ", t.IsBucket, t.Bin);
            }
            Console.WriteLine();
        }

        private void Resize()
        {
            //Console.WriteLine("Resize.");
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
