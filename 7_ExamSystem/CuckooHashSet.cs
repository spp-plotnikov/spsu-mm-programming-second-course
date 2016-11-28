using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CuckooHashSet
{
    private volatile int size;
    protected int probeSize;
    protected int threShold;
    private int limit = 20; //number of attempts to relocate element before giving up
    protected volatile List<Tuple<long, long>>[,] table;
    public CuckooHashSet(int size)
    {
        this.probeSize = 100;
        this.threShold = probeSize / 4 * 3;
        this.size = size;
        this.table = new List<Tuple<long, long>>[2, size];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < size; j++)
            {
                table[i, j] = new List<Tuple<long, long>>(probeSize);
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
                List<Tuple<long, long>> secondSet = table[1, secondHash(x) % size];
                if(secondSet.Contains(x))
                {
                    secondSet.Remove(x);
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
        long firstH = firstHash(x) % size, secondH = secondHash(x) % size;
        int i = -1;
        long h = -1;
        bool mustResize = false;
        try
        {
            List<Tuple<long, long>> firstSet = table[0, firstH];
            List<Tuple<long, long>> secondSet = table[1, secondH];
            if(firstSet.Contains(x) || secondSet.Contains(x))
            {
                return false;
            }
            if(firstSet.Count() < threShold)
            {
                firstSet.Add(x);
                return true;
            }
            else if(secondSet.Count() < threShold)
            {
                secondSet.Add(x);
                return true;
            }
            else if(firstSet.Count() < probeSize)
            {
                firstSet.Add(x);
                i = 0;
                h = firstH;
            }
            else if(secondSet.Count() < probeSize)
            {
                secondSet.Add(x);
                i = 1;
                h = secondH;
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
        for (int round = 0; round < limit; round++)
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
                    if(jSet.Count() < threShold)
                    {
                        jSet.Add(y);
                        return true;
                    }
                    else if(jSet.Count() < probeSize)
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
                else if(iSet.Count() >= threShold)
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
