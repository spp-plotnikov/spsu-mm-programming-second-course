using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Future
{
    class Program
    {
        public static int ThreadPoolSum(int[] array)
        {
            int threadNum = 5;
            int running = threadNum;
            int len = array.Length;
            int ans = 0;
            AutoResetEvent done = new AutoResetEvent(false);
            for (int i = 0; i < threadNum; ++i)
            {
                int subLen = (i < threadNum - 1) ? (len / threadNum) : (len / threadNum + len % threadNum);
                int subBegin = (array.Length / threadNum) * i;
                int[] subArray = new int[subLen];
                Array.Copy(array, subBegin, subArray, 0, subLen);
                ThreadPool.QueueUserWorkItem(state => {
                    int[] curArray = (int[])state;

                    int curSum = curArray.Sum();

                    ans += curSum;

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

                Thread leftThread = new Thread(() =>{
                    leftSum = RecursiveSum(left);
                });
                Thread rightThread = new Thread(() =>{
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

        static void Main(string[] args)
        {
            // example array
            int[] arr = new int[100000000];
            for (int i = 0; i < 10123210; i++)
                arr[i] = 1;

            // summing class
            //AsyncArraySum adder = new AsyncArraySum(ThreadPoolSum);
            AsyncArraySum adder = new AsyncArraySum(RecursiveSum); // more then 1000 - out of memory


            // start summing 
            adder.Sum(arr);

            // for test 
            Console.WriteLine("Waiting main thread tasks");
            //Thread.Sleep(5000);
            Console.WriteLine("Done main thread tasks, waiting summing...");

            // getting sum, if sum was not calc yet - waiting
            int sum = adder.GetSum();

            Console.WriteLine("Sum of array: " + sum);

            Console.ReadKey();
        }
    }
}
