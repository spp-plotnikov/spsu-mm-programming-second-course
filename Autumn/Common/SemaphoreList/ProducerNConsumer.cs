using System;
using System.Threading;

public class Producer
{
    public static void StartCycle<T>(ListWSemaphore<T> list, T elem) // The first param is list and the second is object to add
    {
        while (list.CurNumOfThreads >= 0) // While list is available to changes do something
        {
            list.Add(elem);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name);
            Thread.Sleep(200);
        }
    }
}

public class Consumer
{
    public static void StartCycle<T>(ListWSemaphore<T> list, int index) // The first param is list and the second is object index to remove
    {
        while (list.CurNumOfThreads >= 0) // While list is available to changes do something
        {
            list.RemoveAt(index);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name); 
            Thread.Sleep(200);
        }
    }
}
