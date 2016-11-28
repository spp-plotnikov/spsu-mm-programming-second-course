using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CuckooHashSet
{
    private volatile int size;
    protected int PROBE_SIZE;
    protected int THRESHOLD;
    private int LIMIT = 20; //number of attempts to relocate element before giving up
    protected volatile List<Tuple<long, long>>[,] table;
    public CuckooHashSet(int size)
    {
        this.PROBE_SIZE = 100;
        this.THRESHOLD = PROBE_SIZE / 4 * 3;
        this.size = size;
        this.table = new List<Tuple<long, long>>[2, size];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < size; j++)
            {
                table[i, j] = new List<Tuple<long, long>>(PROBE_SIZE);
            }
        }
    }

    protected long firstHash(Tuple<long, long> elem)
    {
        return (elem.Item1 % 1001) + (elem.Item2 % 113);
    }
    protected long secondHash(Tuple<long, long> elem)
    {
        return (elem.Item1 % 10001) + (elem.Item2 % 101);
    }

    protected abstract void acquire(Tuple<long, long> x);
    protected abstract void release(Tuple<long, long> x);
    protected abstract void resize();

    public bool Remove(Tuple<long, long> x)
    {
        acquire(x);
        try
        {
            List<Tuple<long, long>> firstSet = table[0, firstHash(x) % size];
            if(firstSet.Contains(x))
            {
                firstSet.Remove(x);
                return true;
            }
            else
            {
                List<Tuple<long, long>> set1 = table[1, secondHash(x) % size];
                if(set1.Contains(x))
                {
                    set1.Remove(x);
                    return true;
                }
            }
            return false;
        }
        finally
        {
            release(x);
        }
    }

    public bool Contains(Tuple<long, long> x)
    {
        acquire(x);
        try
        {
            List<Tuple<long, long>> firstList = table[0, firstHash(x) % size];
            if(firstList.Contains(x))
            {
                return true;
            }
            else
            {
                List<Tuple<long, long>> secondList = table[1, secondHash(x) % size];
                if (secondList.Contains(x))
                {
                    return true;
                }
            }
            return false;
        }
        finally
        {
            release(x);
        }
    }

    public bool Add(Tuple<long, long> x)
    {
        acquire(x);
        long h0 = firstHash(x) % size, h1 = secondHash(x) % size;
        int i = -1;
        long h = -1;
        bool mustResize = false;
        try
        {
            List<Tuple<long, long>> set0 = table[0, h0];
            List<Tuple<long, long>> set1 = table[1, h1];
            if(set0.Contains(x) || set1.Contains(x))
            {
                return false;
            }
            if(set0.Count() < THRESHOLD)
            {
                set0.Add(x);
                return true;
            }
            else if(set1.Count() < THRESHOLD)
            {
                set1.Add(x);
                return true;
            }
            else if(set0.Count() < PROBE_SIZE)
            {
                set0.Add(x);
                i = 0;
                h = h0;
            }
            else if(set1.Count() < PROBE_SIZE)
            {
                set1.Add(x);
                i = 1;
                h = h1;
            }
            else
            {
                mustResize = true;
            }
        }
        finally
        {
            release(x);
        }
        if(mustResize)
        {
            resize();
            Add(x);
        }
        else if(!Relocate(i, h))
        {
            resize();
        }
        return true;
    }

    public bool Relocate(int i, long hi)
    {
        long hj = 0;
        int j = 1 - i;
        for (int round = 0; round < LIMIT; round++)
        {
            List<Tuple<long, long>> iSet = table[i, hi];
            Tuple<long, long> y = iSet.ElementAt(0);
            switch (i)
            {
                case 0:
                {
                    hj = secondHash(y) % size;
                    break;
                }
                case 1:
                {
                    hj = firstHash(y) % size;
                    break;
                }
            }
            acquire(y);
            List<Tuple<long, long>> jSet = table[j, hj];
            try
            {
                if(iSet.Remove(y))
                {
                    if(jSet.Count() < THRESHOLD)
                    {
                        jSet.Add(y);
                        return true;
                    }
                    else if(jSet.Count() < PROBE_SIZE)
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
                else if(iSet.Count() >= THRESHOLD)
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
                release(y);
            }
        }
        return false;
    }
}
