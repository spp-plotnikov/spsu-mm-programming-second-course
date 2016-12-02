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
        private Func<int[], int> calculating;

        public AsyncCalculating(Func<int[], int> calculating)
        {
            this.calculating = calculating;
        }
                       
        public void CalculateSum(int[] array)
        {
            Thread taskThread = new Thread(() =>
            {
                sumOfElements = calculating(array);
                isReady.Set();
            });
            taskThread.Start();
        }

        public int Sum
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
