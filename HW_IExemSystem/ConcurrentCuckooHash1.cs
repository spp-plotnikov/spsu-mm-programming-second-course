using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ForUniversity
{
    class ConcurrentCuckooHash : IExamSystem
    {
        private volatile int _capacity;
        private volatile List<KeyValuePair<long, long>>[,] _table;
        private int _probeSize = 73;
        private int _threShold = 42;
        private Mutex[,] _locks;
        private int _lenOfLocks;

        public ConcurrentCuckooHash(int size)
        {
            _capacity = size;
            _table = new List<KeyValuePair<long, long>>[2, _capacity];
            _locks = new Mutex[2, _capacity];
            _lenOfLocks = _capacity;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _capacity; j++)
                {
                    _table[i, j] = new List<KeyValuePair<long, long>>(_probeSize);
                    _locks[i, j] = new Mutex();
                }
            }
        }

        private long GetFirstHash (long studentId, long courseId)
        {
            return (studentId * 1023 + courseId) % (long)1e9 + 9;
        }

        private long GetSecondHash (long studentId, long courseId)
        {
            return (courseId * 2047 + studentId) % (long)1e9 + 7;
        }

        private void Aquire(long studentId, long courseId)
        {
            _locks[0, GetFirstHash(studentId, courseId) % _lenOfLocks].WaitOne();
            _locks[1, GetSecondHash(studentId, courseId) % _lenOfLocks].WaitOne();
        }

        private void Release(long studentId, long courseId)
        {
            _locks[0, GetFirstHash(studentId, courseId) % _lenOfLocks].ReleaseMutex();
            _locks[1, GetSecondHash(studentId, courseId) % _lenOfLocks].ReleaseMutex();
        }

        public void Add(long studentId, long courseId)
        {
            Aquire(studentId, courseId);
            long firstHash = GetFirstHash(studentId, courseId) % _capacity;
            long secondHash = GetSecondHash(studentId, courseId) % _capacity;
            int i = -1;
            long h = -1;
            bool mustResize = false;
            try
            {
                List<KeyValuePair<long, long>> set0 = _table[0, firstHash];
                List<KeyValuePair<long, long>> set1 = _table[1, secondHash];
                
                if (set0.Contains(new KeyValuePair<long, long> (studentId, courseId)) || set1.Contains(new KeyValuePair<long, long> (studentId, courseId)))
                {
                    return;
                }

                if (set0.Count() < _threShold)
                {
                    set0.Add(new KeyValuePair<long, long> (studentId, courseId));
                    return;
                }
                else if (set1.Count() < _threShold)
                {
                    set1.Add(new KeyValuePair<long, long>(studentId, courseId));
                    return;
                }
                else if (set0.Count() < _probeSize)
                {
                    set0.Add(new KeyValuePair<long, long>(studentId, courseId));
                    i = 0;
                    h = firstHash;
                }
                else if (set1.Count() < _probeSize)
                {
                    set1.Add(new KeyValuePair<long, long>(studentId, courseId));
                    i = 1;
                    h = secondHash;
                }
                else
                {
                    mustResize = true;
                }
            }
            finally
            {
                Release(studentId, courseId);
            }
            if (mustResize)
            {
                Resize();
                Add(studentId, courseId);
            }
            else if (!Relocate(i, h))
            {
                Resize();
            }
            return;
        }

        public void Remove(long studentId, long courseId)
        {
            Aquire(studentId, courseId);
            try
            {
                List<KeyValuePair<long, long>> set0 = _table[0, GetFirstHash(studentId, courseId) % _capacity];
                if (set0.Contains(new KeyValuePair<long, long>(studentId, courseId)))
                {
                    set0.Remove(new KeyValuePair<long, long>(studentId, courseId));
                    return;
                }
                else
                {
                    List<KeyValuePair<long, long>> set1 = _table[1, GetSecondHash(studentId, courseId) % _capacity];
                    if (set1.Contains(new KeyValuePair<long, long>(studentId, courseId)))
                    {
                        set1.Remove(new KeyValuePair<long, long>(studentId, courseId));
                        return;
                    }
                }
                return;
            }
            finally
            {
                Release(studentId, courseId);
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            Aquire(studentId, courseId);
            try
            {
                List<KeyValuePair<long, long>> set0 = _table[0, GetFirstHash(studentId, courseId) % _capacity];
                if (set0.Contains(new KeyValuePair<long, long>(studentId, courseId)))
                {
                    return true;
                }
                else
                {
                    List<KeyValuePair<long, long>> set1 = _table[1, GetSecondHash(studentId, courseId) % _capacity];
                    if (set1.Contains(new KeyValuePair<long, long>(studentId, courseId)))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(studentId, courseId);
            }
        }

        private void Resize()
        {
            int oldCapacity = _capacity;
            for (int i = 0; i < _locks.GetLength(0); i++)
            {
                _locks[0, i].WaitOne();
            }

            try
            {
                if (_capacity != oldCapacity)
                {
                    return;
                }
                List<KeyValuePair<long, long>>[,] oldTable = _table;
                _capacity = 2 * _capacity;
                _table = new List<KeyValuePair<long, long>>[2, _capacity];
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < _capacity; j++)
                    {
                        _table[i, j] = new List<KeyValuePair<long, long>>(_probeSize);
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < _capacity / 2; j++)
                    {
                        foreach (KeyValuePair<long, long> oldStudent in oldTable[i, j])
                        {
                            _table[0, GetFirstHash(oldStudent.Key, oldStudent.Value) % _capacity].Add(oldStudent);
                        }
                    }
                }
            }
            finally
            {
                for (int i = 0; i < _locks.GetLength(0); i++)
                {
                    _locks[0, i].ReleaseMutex();
                }
            }
        }

        public bool Relocate(int i, long hi)
        {
            long hj = 0;
            int j = 1 - i;
            for (int k = 0; k < 20; k++)
            {
                List<KeyValuePair<long, long>> iSet = _table[i, hi];
                KeyValuePair<long, long> y = iSet.ElementAt(0);
                switch (i)
                {
                    case 0:
                        {
                            hj = GetFirstHash(y.Key, y.Value) % _capacity;
                            break;
                        }
                    case 1:
                        {
                            hj = GetSecondHash(y.Key, y.Value) % _capacity;
                            break;
                        }
                }
                Aquire(y.Key, y.Value);
                List<KeyValuePair<long, long>> jSet = _table[j, hj];
                try
                {
                    if (iSet.Remove(y))
                    {
                        if (jSet.Count() < _threShold)
                        {
                            jSet.Add(y);
                            return true;
                        }
                        else if (jSet.Count() < _probeSize)
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
                    else if (iSet.Count() >= _threShold)
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
                    Release(y.Key, y.Value);
                }
            }
            return false;
        }
    }
}
