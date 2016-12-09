using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


class SumImplementation
{
    public static int ThreadPoolSum(int[] array)
    {
        int threadNum = 5;
        int running = threadNum;
        int len = array.Length;
        int ans = 0;
        AutoResetEvent done = new AutoResetEvent(false);

        Mutex blockInc = new Mutex();
        for (int i = 0; i < threadNum; ++i)
        {
            int subLen = (i < threadNum - 1) ? (len / threadNum) : (len / threadNum + len % threadNum);
            int subBegin = (array.Length / threadNum) * i;
            int[] subArray = new int[subLen];
            Array.Copy(array, subBegin, subArray, 0, subLen);
            ThreadPool.SetMinThreads(20, 20);
            ThreadPool.QueueUserWorkItem(state => {
                int[] curArray = (int[])state;

                int curSum = curArray.Sum();

                blockInc.WaitOne();

                ans += curSum;

                blockInc.ReleaseMutex();

                if (0 == Interlocked.Decrement(ref running))
                    done.Set();
            }, subArray);
        }
        done.WaitOne();
        return ans;
    }


    public static int RecursiveSum(int[] array)
    {
        if (array.Length > 1)
        {
            int leftSum = 0, rightSum = 0;
            int mid = array.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[mid + array.Length % 2];
            Array.Copy(array, left, mid);
            Array.Copy(array, mid, right, 0, mid + array.Length % 2);

            Thread leftThread = new Thread(() => {
                leftSum = RecursiveSum(left);
            });
            Thread rightThread = new Thread(() => {
                rightSum = RecursiveSum(right);
            });

            leftThread.Start();
            rightThread.Start();
            leftThread.Join();
            rightThread.Join();

            return leftSum + rightSum;
        }
        return array[0];
    }
}

