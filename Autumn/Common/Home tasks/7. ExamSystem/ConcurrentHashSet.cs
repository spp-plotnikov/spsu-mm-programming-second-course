using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExamSystem
{
    class ConcurrentHashSet
    {
        private readonly int probeSize = 4;
        private readonly int threshOld = 2;
        private readonly int numLocks = 25;
        private readonly int limit = 25;
        private volatile int capacity;
        private volatile List<Tuple<long, long>>[] firstExams;
        private volatile List<Tuple<long, long>>[] secondExams;
        private Mutex[] firstMutex;
        private Mutex[] secondMutex;

        public ConcurrentHashSet(int cap)
        {
            capacity = cap;
            firstExams = new List<Tuple<long, long>>[capacity];
            secondExams = new List<Tuple<long, long>>[capacity];
            firstMutex = new Mutex[capacity];
            secondMutex = new Mutex[capacity];
            
            for (int j = 0; j < capacity; j++)
            {
                firstExams[j] = new List<Tuple<long, long>>(probeSize);
                secondExams[j] = new List<Tuple<long, long>>(probeSize);
                firstMutex[j] = new Mutex();
                secondMutex[j] = new Mutex();
            }
        }

        private long FirstHash(Tuple<long, long> exam)
        {
            long first = exam.Item1.GetHashCode();
            long second = exam.Item2.GetHashCode();
            return ((first * 239 + second) % 2003) % 179;
        }

        private long SecondHash(Tuple<long, long> exam)
        {
            long first = exam.Item1.GetHashCode();
            long second = exam.Item2.GetHashCode();
            return ((first * 577 + second) % 1003) % 91;
        }

        private void Aquire(Tuple<long, long> tmp)
        {
            firstMutex[FirstHash(tmp) % numLocks].WaitOne();
            secondMutex[SecondHash(tmp) % numLocks].WaitOne();
        }

        private void Release(Tuple<long, long> tmp)
        {
            firstMutex[FirstHash(tmp) % numLocks].ReleaseMutex();
            secondMutex[SecondHash(tmp) % numLocks].ReleaseMutex();
        }

        private void Resize()
        {
            int oldCapacity = capacity;
            for (int i = 0; i < firstMutex.Length; i++)
            {
                firstMutex[i].WaitOne();
            }

            try
            {
                if (capacity != oldCapacity)
                {
                    return;
                }
                List<Tuple<long, long>>[] firstOldStories = firstExams;
                List<Tuple<long, long>>[] secondOldStories = secondExams;

                capacity *= 2;
                firstExams = new List<Tuple<long, long>>[capacity];
                secondExams = new List<Tuple<long, long>>[capacity];

                for (int j = 0; j < capacity; j++)
                {
                    firstExams[j] = new List<Tuple<long, long>>(probeSize);
                    secondExams[j] = new List<Tuple<long, long>>(probeSize);
                }
                

                
                for (int j = 0; j < capacity / 2; j++)
                {
                    foreach (Tuple<long, long> oldExam in firstOldStories[j])
                    {
                        firstExams[FirstHash(oldExam) % capacity].Add(oldExam);
                    }

                    foreach (Tuple<long, long> oldExam in secondOldStories[j])
                    {
                        firstExams[FirstHash(oldExam) % capacity].Add(oldExam);
                    }
                }
                
            }
            finally
            {
                for (int i = 0; i < firstMutex.Length; i++)
                {
                    firstMutex[i].ReleaseMutex();
                }
            }
        }

        public bool Contains(Tuple<long, long> exam)
        {
            Aquire(exam);
            try
            {
                List<Tuple<long, long>> firstSet = firstExams[FirstHash(exam) % capacity];
                if (firstSet.Contains(exam))
                {
                    return true;
                }
                else
                {
                    List<Tuple<long, long>> secondSet = secondExams[SecondHash(exam) % capacity];
                    if (secondSet.Contains(exam))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(exam);
            }
        }

        public bool Remove(Tuple<long, long> exam)
        {
            Aquire(exam);
            try
            {
                List<Tuple<long, long>> firstSet = firstExams[FirstHash(exam) % capacity];
                if (firstSet.Contains(exam))
                {
                    firstSet.Remove(exam);
                    return true;
                }
                else
                {
                    List<Tuple<long, long>> secondSet = secondExams[SecondHash(exam) % capacity];
                    if (secondSet.Contains(exam))
                    {
                        secondSet.Remove(exam);
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(exam);
            }
        }

        public bool Add(Tuple<long, long> exam)
        {
            Aquire(exam);

            long hashFirst = FirstHash(exam) % capacity;
            long hashSecond = SecondHash(exam) % capacity;

            int res = -1;
            long hash = -1;
            bool isResize = false;
            try
            {
                List<Tuple<long, long>> firstSet = firstExams[hashFirst];
                List<Tuple<long, long>> secondSet = secondExams[hashSecond];
                
                if (firstSet.Contains(exam) || secondSet.Contains(exam))
                {
                    return false;
                }

                if (firstSet.Count() < threshOld)
                {
                    firstSet.Add(exam);
                    return true;
                }
                else if (secondSet.Count() < threshOld)
                {
                    secondSet.Add(exam);
                    return true;
                }
                else if (firstSet.Count() < probeSize)
                {
                    firstSet.Add(exam);
                    res = 0;
                    hash = hashFirst;
                }
                else if (secondSet.Count() < probeSize)
                {
                    secondSet.Add(exam);
                    res = 1;
                    hash = hashSecond;
                }
                else
                {
                    isResize = true;
                }
            }
            finally
            {
                Release(exam);
            }

            if (isResize)
            {
                Resize();
                Add(exam);
            }
            else if (!Relocate(res, hash))
            {
                Resize();
            }
            return true;
        }

        public bool Relocate(int i, long hi)
        {
            long hj = 0;
            int j = 1 - i;
            for (int k = 0; k < limit; k++)
            {
                List<Tuple<long, long>> iSet;
                if (i == 0)
                {
                    iSet = firstExams[hi];
                }
                else
                {
                    iSet = secondExams[hi];
                }

                if (i == 0)
                {
                    firstMutex[hi % numLocks].WaitOne();
                }
                else
                {
                    secondMutex[hi % numLocks].WaitOne();
                }

                if (iSet.Count == 0)
                {
                    return true;
                }

                Tuple<long, long> y = iSet.ElementAt(0);

                if (i == 0)
                {
                    firstMutex[hi % numLocks].ReleaseMutex();
                }
                else
                {
                    secondMutex[hi % numLocks].ReleaseMutex();
                }

                Aquire(y);
                switch (i)
                {
                    case 0:
                        {
                            hj = FirstHash(y) % capacity;
                            break;
                        }
                    case 1:
                        {
                            hj = SecondHash(y) % capacity;
                            break;
                        }
                }
                List<Tuple<long, long>> jSet;
                if (j == 0)
                {
                    jSet = firstExams[hj];
                }
                else
                {
                    jSet = secondExams[hj];
                }
                try
                {
                    if (iSet.Remove(y))
                    {
                        if (jSet.Count() < threshOld)
                        {
                            jSet.Add(y);
                            return true;
                        }
                        else if (jSet.Count() < probeSize)
                        {
                            jSet.Add(y);
                            i = 1 - i;
                            hi = hj;
                            j = 1 - j;
                        }
                        else
                        {
                            iSet.Add(y);
                            return false;
                        }
                    }
                    else if (iSet.Count() >= threshOld)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
                finally
                {
                    Release(y);
                }
            }
            return false;
        }
    }
}
