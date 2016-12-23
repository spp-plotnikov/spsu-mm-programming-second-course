using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            List<KeyValuePair<long, long>> firstSet = null;
            List<KeyValuePair<long, long>> secondSet = null;
            bool result = false;
            try
            {
                long h1 = FirstHash(x) % _capacity;
                long h2 = SecondHash(x) % _capacity;
                firstSet = Table[0, h1];
                secondSet = Table[1, h2];
                Acquire(firstSet);
                Acquire(secondSet);
                if (firstSet.Contains(x) || secondSet.Contains(x))
                {
                    result = true;
                }
            }
            finally
            {
                Release(firstSet);
                Release(secondSet);
                Release(x);
            }
            return result;
        }

        public bool Add(KeyValuePair<long, long> x)
        {
            Acquire(x);
            long h1 = FirstHash(x) % _capacity;
            long h2 = SecondHash(x) % _capacity;
            long i = -1;
            long h = -1;
            bool needResize = false;
            List<KeyValuePair<long, long>> firstSet = null;
            List<KeyValuePair<long, long>> secondSet = null;
            try
            {
                firstSet = Table[0, h1];
                secondSet = Table[1, h2];
                Acquire(firstSet);
                Acquire(secondSet);
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
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Release(firstSet);
                Release(secondSet);
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
            long h1 = FirstHash(x) % _capacity;
            long h2 = SecondHash(x) % _capacity;
            List<KeyValuePair<long, long>> firstSet = null;
            List<KeyValuePair<long, long>> secondSet = null;
            bool result = false;
            try
            {
                firstSet = Table[0, h1];
                secondSet = Table[1, h2];
                Acquire(firstSet);
                Acquire(secondSet);
                if (firstSet.Contains(x))
                {
                    firstSet.Remove(x);
                    result = true;
                }
                else
                {
                    if (secondSet.Contains(x))
                    {
                        secondSet.Remove(x);
                        result = true;
                    }
                }
            }
            finally
            {
                Release(firstSet);
                Release(secondSet);
                Release(x);
            }
            return result;
        }

        protected bool Relocate(long i, long hi)
        {
            long hj = 0;
            long j = 1 - i;
            for (int round = 0; round < _limit; round++)
            {
                List<KeyValuePair<long, long>> iSet;
                Acquire(iSet = Table[i, hi]);
                KeyValuePair<long, long> y = iSet.First();
                Release(iSet);

                switch (i)
                {
                    case 0:
                        hj = SecondHash(y) % _capacity;
                        break;
                    case 1:
                        hj = FirstHash(y) % _capacity;
                        break;
                }
                Acquire(y);
                List<KeyValuePair<long, long>> jSet = Table[j, hj];

                try
                {
                    if (iSet.Remove(y))
                    {
                        if (jSet.Count < _thresHold)
                        {
                            jSet.Add(y);
                            return true;
                        }
                        else if (jSet.Count < ProbSize)
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
                    else if (iSet.Count >= _thresHold)
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

        public long FirstHash(KeyValuePair<long, long> x)
        {
            return x.Key % 243 + x.Value % 53;
        }

        public long SecondHash(KeyValuePair<long, long> x)
        {
            return x.Key % 257 + x.Value % 57;
        }

        public abstract void Acquire(KeyValuePair<long, long> x);
        public abstract void Acquire(List<KeyValuePair<long, long>> x);

        public abstract void Release(KeyValuePair<long, long> x);
        public abstract void Release(List<KeyValuePair<long, long>> x);


        public abstract void Resize();
    }
}
