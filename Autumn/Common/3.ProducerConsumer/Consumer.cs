using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class Consumer
{
    private Thread thread;
    private List<int> sharedData;
    private bool runnable = false;
    private const int pause = 2000;

    public Consumer(List<int> sharedData, Locker lockFlag)
    {
        this.sharedData = sharedData;
        this.runnable = true;
        this.thread = new Thread(() => Run(lockFlag));
        this.thread.Start();
    }

    public void Stop()
    {
        this.runnable = false;
        this.thread.Join();
    }

    public void Run(Locker lockFlag)
    {
        while (this.runnable)
        {
            lockFlag.Lock();
            // Stil running ?
            if (this.runnable)
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
            lockFlag.Release();
            Thread.Sleep(pause);
        }
    }
}
