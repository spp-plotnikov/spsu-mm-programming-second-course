using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class AsyncArraySum : ISumOfElements
{
    private int[] arr;
    Func<int[], int> sumFuntion;
    private Thread summingThread;
    private int sum;

    public AsyncArraySum(Func<int[], int> sumFuntion)
    {
        this.sumFuntion = sumFuntion;
        this.sum = 0;
    }

    public int Sum(int[] arr)
    {
        this.arr = arr;
        summingThread = new Thread(DoSumming);
        summingThread.Start();

        summingThread.Join();
        Console.WriteLine("Array was summed successfully!");
        return this.sum;
    }


    private void DoSumming()
    {
        this.sum = this.sumFuntion(this.arr);
    }
}
