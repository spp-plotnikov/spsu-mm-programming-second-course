using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Decanat
{
    // algorithm copied from the book with slight changes
    class ConcurrentCuckooHashSet
    {
        private int _probeSize = 100;
        private int _threShold = 75;
        private int _lenLocks = 10;
        private volatile int _capacity;
        private volatile List<KeyValuePair<long, long>> [,] _studentsStories;
        private Mutex[,] _mutexLocks;

        public ConcurrentCuckooHashSet(int capacity)
        {
            _capacity = capacity;
            _studentsStories = new List<KeyValuePair<long, long>>[2, capacity];
            _mutexLocks = new Mutex[2, capacity];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < capacity; j++)
                {
                    _studentsStories[i, j] = new List<KeyValuePair<long, long>>(_probeSize);
                    _mutexLocks[i, j] = new Mutex();
                }
            }
        }

        private long HashOne(KeyValuePair<long, long> tmp)
        {
            return ((tmp.Key * 997 + tmp.Value) % 1001) % 103;
        }

        private long HashTwo(KeyValuePair<long, long> tmp)
        {
            return (((tmp.Key + tmp.Value) % 823) % 89);
        }

        private void Aquire (KeyValuePair<long, long> tmp)
        {
            _mutexLocks[0, HashOne(tmp) % _lenLocks].WaitOne();
            _mutexLocks[1, HashTwo(tmp) % _lenLocks].WaitOne();
        }

        private void Release (KeyValuePair<long, long> tmp)
        {
            _mutexLocks[0, HashOne(tmp) % _lenLocks].ReleaseMutex();
            _mutexLocks[1, HashTwo(tmp) % _lenLocks].ReleaseMutex();
        }

        private void Resize()
        {
            int oldCapacity = _capacity;
            for (int i = 0; i < _mutexLocks.GetLength(0); i++ )
            {
                _mutexLocks[0, i].WaitOne();
            }

            try
            {
                if (_capacity != oldCapacity)
                {
                    return;
                }
                List<KeyValuePair<long, long>>[,] oldStories = _studentsStories;
                _capacity = 2 * _capacity;
                _studentsStories = new List<KeyValuePair<long, long>>[2, _capacity];
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < _capacity; j++)
                    {
                        _studentsStories[i, j] = new List<KeyValuePair<long, long>>(_probeSize);
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < _capacity / 2; j++)
                    {
                        foreach (KeyValuePair<long, long> oldStory in oldStories[i, j])
                        {
                            _studentsStories[0, HashOne(oldStory) % _capacity].Add(oldStory);
                        }
                    }
                }
            }
            finally
            {
                for (int i = 0; i < _mutexLocks.GetLength(0); i++)
                {
                    _mutexLocks[0, i].ReleaseMutex();
                }
            }
        }

        public bool Contains(KeyValuePair<long, long> toCheck)
        {
            Aquire(toCheck);
            try
            {
                List<KeyValuePair<long, long>> set = _studentsStories[0, HashOne(toCheck) % _capacity];
                if (set.Contains(toCheck))
                {
                    return true;
                }
                else
                {
                    List<KeyValuePair<long, long>> setNew = _studentsStories[1, HashTwo(toCheck) % _capacity];
                    if (setNew.Contains(toCheck))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(toCheck);
            }
        }

        public bool Remove(KeyValuePair<long, long> toRemove)
        {
            Aquire(toRemove);
            try
            {
                List<KeyValuePair<long, long>> set = _studentsStories[0, HashOne(toRemove) % _capacity];
                if (set.Contains(toRemove))
                {
                    set.Remove(toRemove);
                    return true;
                }
                else
                {
                    List<KeyValuePair<long, long>> setNew = _studentsStories[1, HashTwo(toRemove) % _capacity];
                    if (setNew.Contains(toRemove))
                    {
                        setNew.Remove(toRemove);
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(toRemove);
            }
        }

        public bool Add(KeyValuePair<long, long> toAdd)
        {
            Aquire(toAdd);
            long hashFi = HashOne(toAdd) % _capacity;
            long hashSe = HashTwo(toAdd) % _capacity;
            int i = -1;
            long h = -1;
            bool mustResize = false;
            try
            {
                List<KeyValuePair<long, long>> set = _studentsStories[0, hashFi];
                List<KeyValuePair<long, long>> setNew = _studentsStories[1, hashSe];

                // no need to add it
                if (set.Contains(toAdd) || setNew.Contains(toAdd))
                {
                    return false;
                }

                if (set.Count() < _threShold)
                {
                    set.Add(toAdd);
                    return true;
                }
                else if (setNew.Count() < _threShold)
                {
                    setNew.Add(toAdd);
                    return true;
                }
                else if (set.Count() < _probeSize)
                {
                    set.Add(toAdd);
                    i = 0;
                    h = hashFi;
                }
                else if (setNew.Count() < _probeSize)
                {
                    setNew.Add(toAdd);
                    i = 1;
                    h = hashSe;
                }
                else
                {
                    mustResize = true;
                }
            }
            finally
            {
                Release(toAdd);
            }
            if (mustResize)
            {
                Resize();
                Add(toAdd);
            }
            else if (!Relocate(i, h))
            {
                Resize();
            }
            return true;
        }

        public bool Relocate(int i, long hi)
        {
            long hj = 0;
            int j = 1 - i;
            for (int k = 0; k < 20; k++)
            {
                List<KeyValuePair<long, long>> iSet = _studentsStories[i, hi];
                KeyValuePair<long, long> y = iSet.ElementAt(0);
                switch(i)
                {
                    case 0:
                    {
                        hj = HashOne(y) % _capacity;
                        break;
                    }
                    case 1:
                    {
                        hj = HashTwo(y) % _capacity;
                        break;
                    }
                }
                Aquire(y);
                List<KeyValuePair<long, long>> jSet = _studentsStories[j, hj];
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
                    Release(y);
                }
            }
            return false;
        }
    }
}
