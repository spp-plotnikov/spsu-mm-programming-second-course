using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{

    class BOrganisation : IExamSystem
    {
        ListItem total;
        Bucket[] buckets;
        int _tSize = 8;
        int _lSize = 4;
        bool _flag = true;

        public BOrganisation()
        {
            total = new Bucket(0);
            buckets = new Bucket[_tSize];
            for (int i = 0; i < _tSize; i++)
                buckets[i] = new Bucket(i);

            buckets[0].Next = total;
        }

        public void Stop()
        {
            _flag = false;
        }
        private void AddBucket(int number)
        {
            ListItem preNode = total;
            ListItem node = preNode.Next;
            Bucket newBuck = new Bucket(number);

            while (node != null)
            {
                if (node.BinFormat.CompareTo(newBuck.BinFormat) < 0)
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
            while (node.BinFormat.CompareTo(pred.BinFormat) <= 0)
            {
                if (node == pred)
                    return pred.Next == curr;
                node = node.Next;
                if (node == null) break;
            }
            return false;
        }

        public void Add(long studentId, long courseId)
        {
            StudInfo curSt = new StudInfo(studentId, courseId);
            int hash = curSt.Hash % _tSize;
            if (buckets[hash].Next == null)
                AddBucket(hash);

            while (true)
            {
                int len = 0;
                ListItem preNode = buckets[hash].Next;
                ListItem node = preNode.Next;
                while (node != null && node.BinFormat.CompareTo(curSt.BinFormat) < 0) 
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
                if (len > _lSize)
                {
                    Resize();
                    Add(curSt.StudentID, curSt.CourseID);
                    return;
                }

                preNode.BucketMutex.WaitOne();
                node.BucketMutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, hash))
                        continue;
                    if (node.BinFormat.CompareTo(curSt.BinFormat) == 0)
                        return;
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
            StudInfo curSt = new StudInfo(studentId, courseId);
            int hash = curSt.Hash % _tSize;
            
            if (buckets[hash].Next == null)
            {
                AddBucket(hash);
            }

            while (true)
            {
                ListItem preNode = buckets[hash].Next;
                ListItem node = preNode.Next;
                while (node != null && node.BinFormat.CompareTo(curSt.BinFormat) < 0)
                {
                    preNode = node;
                    node = node.Next;
                }

                if (node == null || preNode.IsBucket && node.IsBucket)
                {
                    return false;
                }
                if (_flag)
                {
                    preNode.BucketMutex.WaitOne();
                    node.BucketMutex.WaitOne();


                    try
                    {
                        if (!Validate(preNode, node, hash)) continue;
                        return node.BinFormat.Equals(curSt.BinFormat);
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {

                        preNode.BucketMutex.ReleaseMutex();
                        node.BucketMutex.ReleaseMutex();



                    }
                }
            }
        }

        public void Remove(long studentId, long courseId)
        {
            StudInfo curSt = new StudInfo(studentId, courseId);
            int hash = curSt.Hash % _tSize;
            if (buckets[hash].Next == null)
                AddBucket(hash);

            while (true)
            {
                ListItem preNode = buckets[hash].Next;
                ListItem node = preNode.Next;
                while (node != null && node.BinFormat.CompareTo(curSt.BinFormat) < 0) 
                {
                    preNode = node;
                    node = node.Next;
                }
                if (node == null || preNode.IsBucket && node.IsBucket)
                    return;


                preNode.BucketMutex.WaitOne();
                node.BucketMutex.WaitOne();

                try
                {
                    if (!Validate(preNode, node, hash))
                        continue;
                    if (node.BinFormat.CompareTo(curSt.BinFormat) == 0)
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



        private void Resize()
        {
            Thread.Sleep(1000);
            int newSize = _tSize * 2;
            Array.Resize(ref buckets, newSize);
            for (int i = _tSize; i < newSize; i++)
            {
                buckets[i] = new Bucket(i);
            }
            _tSize = newSize;
            _lSize = _tSize / 2;
        }
    }
}
