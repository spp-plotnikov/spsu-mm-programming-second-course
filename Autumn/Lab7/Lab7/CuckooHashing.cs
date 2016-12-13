using System.Collections.Generic;

namespace Lab7
{
    public abstract class CuckooHashing
    {
        volatile int _capacity;
        public int ProbSize;
        int _thresHold;
        int _limit = 15;
        public volatile List<KeyValuePair<long, long>>[,] Table;

        public CuckooHashing(int size)
        {
            _capacity = size;
            ProbSize = 100;
            _thresHold = ProbSize / 2;
            Table = new List<KeyValuePair<long, long>>[2, _capacity];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Table[i, j] = new List<KeyValuePair<long, long>>(ProbSize);
                }
            }
        }

        public bool Contains(KeyValuePair<long, long> x)
        {
            Acquire(x);
            try
            {
                long h1 = firstHash(x) % _capacity;
                long h2 = secondHash(x) % _capacity;
                List<KeyValuePair<long, long>> firstSet = Table[0, h1];
                List<KeyValuePair<long, long>> secondSet = Table[1, h2];
                if (firstSet.Contains(x) || secondSet.Contains(x))
                {
                    return true;
                }
                return false;

            }
            finally
            {
                Release(x);
            }
        }

        public bool Add(KeyValuePair<long, long> x)
        {
            Acquire(x);
            long h1 = firstHash(x) % _capacity;
            long h2 = secondHash(x) % _capacity;
            long i = -1;
            long h = -1;
            bool needResize = false;
            try
            {
                List<KeyValuePair<long, long>> firstSet = Table[0, h1];
                List<KeyValuePair<long, long>> secondSet = Table[1, h2];
                if (firstSet.Contains(x) || secondSet.Contains(x))
                {
                    return false;
                }
                if (firstSet.Count < _thresHold)
                {
                    firstSet.Add(x);
                    return true;
                }
                else if (secondSet.Count < _thresHold)
                {
                    secondSet.Add(x);
                    return true;
                }
                else if (firstSet.Count < ProbSize)
                {
                    firstSet.Add(x);
                    i = 0;
                    h = h1;
                }
                else if (secondSet.Count < ProbSize)
                {
                    secondSet.Add(x);
                    i = 1;
                    h = h2;
                }
                else
                {
                    needResize = true;
                }
            }
            finally
            {
                Release(x);
            }

            if (needResize)
            {
                Resize();
                Add(x);
            }
            else if (!Relocate(i, h))
            {
                Resize();
            }
            return true;
        }

        public bool Remove(KeyValuePair<long, long> x)
        {
            Acquire(x);
            long h1 = firstHash(x) % _capacity;
            long h2 = secondHash(x) % _capacity;
            try
            {
                List<KeyValuePair<long, long>> firstSet = Table[0, h1];
                if (firstSet.Contains(x))
                {
                    firstSet.Remove(x);
                    return true;
                }
                else
                {
                    List<KeyValuePair<long, long>> secondSet = Table[1, h2];
                    if (secondSet.Contains(x))
                    {
                        secondSet.Remove(x);
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(x);
            }
        }

        protected bool Relocate(long i, long hi)
        {
            long hj = 0;
            long j = 1 - i;
            for (int round = 0; round < _limit; round++)
            {
                List<KeyValuePair<long, long>> iSet = Table[i, hi];
                KeyValuePair<long, long> y = iSet[0];
                switch (i)
                {
                    case 0:
                        hj = secondHash(y) % _capacity;
                        break;
                    case 1:
                        hj = firstHash(y) % _capacity;
                        break;
                }
                Acquire(y);
                List<KeyValuePair<long, long>> jSet = Table[j, hj];

                try
                {
                    if (iSet.Remove(y))
                    {
                        if (jSet.Capacity < _thresHold)
                        {
                            jSet.Add(y);
                            return true;
                        }
                        else if (jSet.Capacity < ProbSize)
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
                    else if (iSet.Capacity >= _thresHold)
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

        public long firstHash(KeyValuePair<long, long> x)
        {
            return x.Key % 243 + x.Value % 103;
        }

        public long secondHash(KeyValuePair<long, long> x)
        {
            return x.Key % 257 + x.Value % 97;
        }

        public abstract void Acquire(KeyValuePair<long, long> x);

        public abstract void Release(KeyValuePair<long, long> x);

        public abstract void Resize();
    }
}
