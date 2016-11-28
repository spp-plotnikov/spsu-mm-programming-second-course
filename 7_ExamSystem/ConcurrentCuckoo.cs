using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class ConcurrentCuckoo : CuckooHashSet
{
    private int numLocks;
    private Semaphore[] firstLocks;
    private Semaphore[] secondLocks;
    private int size;
    public ConcurrentCuckoo(int size) : base(size)
    {
        this.size = size;
        this.numLocks = 5;
        this.firstLocks = new Semaphore[size];
        this.secondLocks = new Semaphore[size];
        for (int j = 0; j < size; j++)
        {
            firstLocks[j] = new Semaphore(1, 1);
            secondLocks[j] = new Semaphore(1, 1);
        }
    }

    protected override void Acquire(Tuple<long, long> x)
    {
        firstLocks[firstHash(x) % numLocks].WaitOne();
        secondLocks[secondHash(x) % numLocks].WaitOne();
    }

    protected override void Release(Tuple<long, long> x)
    {
        firstLocks[firstHash(x) % numLocks].Release();
        secondLocks[secondHash(x) % numLocks].Release();
    }
    protected override void Resize()
    {
        int oldSize = size;
        foreach (Semaphore semaphore in firstLocks)
        {
            semaphore.WaitOne();
        }
        try
        {
            if(size != oldSize)
            {
                return;
            }
            List<Tuple<long, long>>[,] oldTable = table;
            size = 2 * size;
            Console.WriteLine(size);
            table = new List<Tuple<long, long>>[2, size];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    table[i, j] = new List<Tuple<long, long>>(probeSize);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < size / 2; j++)
                {
                    foreach (Tuple<long, long> oldX in oldTable[i, j])
                    {
                        table[0, firstHash(oldX) % size].Add(oldX);
                    }
                }
            }
        }
        finally
        {
            foreach (Semaphore semaphore in firstLocks)
            {
                semaphore.Release();
            }
        }
    }
}
