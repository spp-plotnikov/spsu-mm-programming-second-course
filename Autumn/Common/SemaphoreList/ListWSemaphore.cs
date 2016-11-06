using System;
using System.Collections.Generic;
using System.Threading;

public class ListWSemaphore<T> : List<T>
{
    private static int maxNumOfThreads = 2; // Number of maximum concurrent threads
    public int curNumOfThreads = 0; // Number of current threads, working with list
    private List<T> list = new List<T>(); // Just list

    public int CurNumOfThreads // Property
    {
        get
        {
            return curNumOfThreads;
        }
    }

    public void Add(T item) // Adding an item
    {
        while (maxNumOfThreads <= curNumOfThreads) { Console.WriteLine("{0} Waiting, curThreads = {1}", Thread.CurrentThread.Name, curNumOfThreads); Thread.Sleep(100); }
        curNumOfThreads++;
        list.Add(item);
        curNumOfThreads--;
    }

    public void RemoveAt(int index) // Removing an item by index
    {
        while (maxNumOfThreads <= curNumOfThreads) { Console.WriteLine("{0} Waiting, curThreads = {1}", Thread.CurrentThread.Name, curNumOfThreads); Thread.Sleep(100); }
        curNumOfThreads++;
        try
        {
            list.RemoveAt(index);
        }
        catch { }
        curNumOfThreads--;
    }

    public void LockFrvr() // Making object inaccessible to changes
    {
        curNumOfThreads = -1;
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
