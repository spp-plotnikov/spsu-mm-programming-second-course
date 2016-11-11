using System;
using System.Threading;

public class Producer
{
    public static void StartCycle<T>(ListWSemaphore<T> list, T elem) // The first param is list and the second is object to add
    {
        while (!list.endOfWork) // While list is available to changes do something
        {
            list.Add(elem);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name);
            Thread.Sleep(50);
        }
    }
}

public class Consumer
{
    public static void StartCycle<T>(ListWSemaphore<T> list, int index) // The first param is list and the second is object index to remove
    {
        while (!list.endOfWork) // While list is available to changes do something
        {
            list.RemoveAt(index);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name); 
        }
    }
}
