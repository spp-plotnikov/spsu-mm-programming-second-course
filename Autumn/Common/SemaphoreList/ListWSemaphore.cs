using System;
using System.Collections.Generic;
using System.Threading;

public class ListWSemaphore<T> : List<T>
{
    public bool endOfWork = false;
    private static int maxNumOfThreads = SemaphoreExample.Constants.maxNumOfThreads;
    private int curNumOfThreads = 0;
    static Semaphore semaphore = new Semaphore(maxNumOfThreads, maxNumOfThreads);
    private List<T> list = new List<T>(); // Just list
    
    public int CurNumOfThreads
    {
        get
        {
            return curNumOfThreads;
        }
    }

    public void Add(T item) // Adding an item
    {
        if (curNumOfThreads >= maxNumOfThreads) { Console.WriteLine("{0} is waiting", Thread.CurrentThread.Name); }
        semaphore.WaitOne();
        curNumOfThreads++;
        list.Add(item);
        semaphore.Release();
        curNumOfThreads--;
    }

    public void RemoveAt(int index) // Removing an item by index
    {
        if (curNumOfThreads >= maxNumOfThreads) { Console.WriteLine("{0} is waiting", Thread.CurrentThread.Name); }
        semaphore.WaitOne();
        curNumOfThreads++;
        try
        {
            list.RemoveAt(index);
        }
        catch { }
        semaphore.Release();
        curNumOfThreads--;
    }

    public void LockFrvr() // Making object inaccessible to changes
    {
        endOfWork = true;
    }

    public void WriteList() // WriteLine list
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.Write(list[i]);
        }
        Console.WriteLine();
    }
}
