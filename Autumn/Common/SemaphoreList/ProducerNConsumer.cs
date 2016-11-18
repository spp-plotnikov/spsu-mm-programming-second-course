using System;
using System.Threading;

public class Producers
{
    private static bool isWorking = false;
    public void StartProducers<T>(int numOfProducers, ListWSemaphore<T> list, T elem) // Run producers and init its names 
    {
        isWorking = true;
        for (int i = 0; i < numOfProducers; i++)
        {
            Thread producer = new Thread(new ThreadStart(() => StartCycle(list, elem)));
            producer.Name = "Producer #" + i.ToString();
            producer.Start();
        }
    }

    private static void StartCycle<T>(ListWSemaphore<T> list, T elem) // The first param is list and the second is object to add
    {
        while (isWorking) // While list is available to changes do something
        {
            list.Add(elem);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name);
            Thread.Sleep(5);
        }
    }

    public void StopWorking() // Stop produces
    {
        isWorking = false;
    }
}

//**********************************************************************

public class Consumers
{
    private static bool isWorking = false;
    public void StartConsumers<T>(int numOfConsumers, ListWSemaphore<T> list, int index) // // Run consumers and init its names
    {
        isWorking = true;
        for (int i = 0; i < numOfConsumers; i++)
        {
            Thread consumer = new Thread(new ThreadStart(() => StartCycle(list, index)));
            consumer.Name = "Consumer #" + i.ToString();
            consumer.Start();
        }
    }

    private void StartCycle<T>(ListWSemaphore<T> list, int index) // The first param is list and the second is object index to remove
    {
        while (isWorking) // While list is available to changes do something
        {
            list.RemoveAt(index);
            Console.WriteLine("{0} did something", Thread.CurrentThread.Name);
            Thread.Sleep(5);
        }
    }

    public void StopWorking() // Stop consumers
    {
        isWorking = false;
    }
}
