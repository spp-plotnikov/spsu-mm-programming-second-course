using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class ConcurrentCuckoo : CuckooHashSet
{
    private int NumLocks;
    private Semaphore[] Locks0;
    private Semaphore[] Locks1;
    private int Size;
    public ConcurrentCuckoo(int size) : base(size)
    {
        this.Size = size;
        this.NumLocks = 1;
        this.Locks0 = new Semaphore[size];
        this.Locks1 = new Semaphore[size];
        for (int j = 0; j < size; j++)
        {
            Locks0[j] = new Semaphore(1, 1);
            Locks1[j] = new Semaphore(1, 1);
        }
    }

    internal override void Acquire(Tuple<long, long> x)
    {
        Locks0[Hash0(x) % NumLocks].WaitOne();
        Locks1[Hash1(x) % NumLocks].WaitOne();
    }

    internal override void Release(Tuple<long, long> x)
    {
        Locks0[Hash0(x) % NumLocks].Release();
        Locks1[Hash1(x) % NumLocks].Release();
    }
    internal override void Resize()
    {
        int oldSize = Size;
        foreach (Semaphore semaphore in Locks0)
        {
            semaphore.WaitOne();
        }
        try
        {
            if (Size != oldSize)
            {
                return;
            }
            List<Tuple<long, long>>[,] oldTable = Table;
            Size = 2 * Size;
            Console.WriteLine(Size);
            Table = new List<Tuple<long, long>>[2, Size];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Table[i, j] = new List<Tuple<long, long>>(PROBE_SIZE);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < Size / 2; j++)
                {
                    foreach (Tuple<long, long> oldX in oldTable[i, j])
                    {
                        Table[0, Hash0(oldX) % Size].Add(oldX);
                    }
                }
            }
        }
        finally
        {
            foreach (Semaphore semaphore in Locks0)
            {
                semaphore.Release();
            }
        }
    }
}