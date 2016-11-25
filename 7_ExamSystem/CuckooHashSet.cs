using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CuckooHashSet
{
    private volatile int Size;
    internal int PROBE_SIZE;
    internal int THRESHOLD;
    private int LIMIT = 20; //number of attempts to realocate element before giving up
    internal volatile List<Tuple<long, long>>[,] Table;
    public CuckooHashSet(int size)
    {
        this.PROBE_SIZE = 100;
        this.THRESHOLD = PROBE_SIZE / 4 * 3;
        this.Size = size;
        this.Table = new List<Tuple<long, long>>[2, size];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Table[i, j] = new List<Tuple<long, long>>(PROBE_SIZE);
            }
        }
    }

    internal long Hash0(Tuple<long, long> elem)
    {
        return (elem.Item1 % 1001) + (elem.Item2 % 113);
    }
    internal long Hash1(Tuple<long, long> elem)
    {
        return (elem.Item1 % 10001) + (elem.Item2 % 101);
    }

    internal abstract void Acquire(Tuple<long, long> x);
    internal abstract void Release(Tuple<long, long> x);
    internal abstract void Resize();

    public bool Remove(Tuple<long, long> x)
    {
        Acquire(x);
        try
        {
            List<Tuple<long, long>> set0 = Table[0, Hash0(x) % Size];
            if (set0.Contains(x))
            {
                set0.Remove(x);
                return true;
            }
            else
            {
                List<Tuple<long, long>> set1 = Table[1, Hash1(x) % Size];
                if (set1.Contains(x))
                {
                    set1.Remove(x);
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

    public bool Contains(Tuple<long, long> x)
    {
        Acquire(x);
        try
        {
            List<Tuple<long, long>> list0 = Table[0, Hash0(x) % Size];
            if (list0.Contains(x))
            {
                return true;
            }
            else
            {
                List<Tuple<long, long>> list1 = Table[1, Hash1(x) % Size];
                if (list1.Contains(x))
                {
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

    public bool Add(Tuple<long, long> x)
    {
        Acquire(x);
        long h0 = Hash0(x) % Size, h1 = Hash1(x) % Size;
        int i = -1;
        long h = -1;
        bool mustResize = false;
        try
        {
            List<Tuple<long, long>> set0 = Table[0, h0];
            List<Tuple<long, long>> set1 = Table[1, h1];
            if(set0.Contains(x) || set1.Contains(x))
            {
                return false;
            }
            if (set0.Count() < THRESHOLD)
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
            Release(x);
        }
        if(mustResize)
        {
            Resize();
            Add(x);
        }
        else if(!Relocate(i, h))
        {
            Resize();
        }
        return true;
    }

    protected bool Relocate(int i, long hi)
    {
        long hj = 0;
        int j = 1 - i;
        for (int round = 0; round < LIMIT; round++)
        {
            List<Tuple<long, long>> iSet = Table[i, hi];
            Tuple<long, long> y = iSet.ElementAt(0);
            switch (i)
            {
                case 0:
                {
                    hj = Hash1(y) % Size;
                    break;
                }
                case 1:
                {
                    hj = Hash0(y) % Size;
                    break;
                }
            }
            Acquire(y);
            List<Tuple<long, long>> jSet = Table[j, hj];
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
                Release(y);
            }
        }
        return false;
    }
}
