using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Future
{
    class AsyncCalculating : SumOfElements
    {
        private int sumOfElements;
        ManualResetEvent isReady = new ManualResetEvent(false);
                       
        public void CalculateSum(int[] array)
        {
            Thread taskThread = new Thread(() =>
            {
                sumOfElements = ArraySumImplementation.FirstSum(array);
                isReady.Set();
            });
            taskThread.Start();
        }

        public int GetSum
        {
            get
            {
                isReady.WaitOne();
                isReady.Reset();
                return sumOfElements;
            }
        }
    }
}
