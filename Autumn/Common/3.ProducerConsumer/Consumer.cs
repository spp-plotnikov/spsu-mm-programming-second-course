using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class Consumer
{
    Thread Thread;
    List<int> sharedData;
    bool Runnable = false;
    private const int pause = 2000;

    public Consumer(List<int> sharedData)
    {
        this.sharedData = sharedData;
        this.Runnable = true;
        this.Thread = new Thread(Run);
        this.Thread.Start();
    }

    public void Stop()
    {
        this.Runnable = false;
        this.Thread.Join();
    }

    public void Run()
    {
        while (this.Runnable)
        {
            Locker.Lock();
            // Stil running ?
            if (this.Runnable)
            {
                if (sharedData.Count == 0)
                {
                    Console.WriteLine("Consumer: list is empty");
                }
                else
                {
                    int value = sharedData.Last();
                    sharedData.Remove(value);
                    Console.WriteLine("Consumer: removed " + value + " from list");
                }
            }
            Locker.Release();
            Thread.Sleep(pause);
        }
    }
}
