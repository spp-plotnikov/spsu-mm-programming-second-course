using System;
using System.Collections.Generic;
using System.Threading;


namespace Future
{
    class ArraySumImplementation
    {
        public static KeyValuePair<int[], int[]> BisectArray(int[] array) // Slicing array into 2 parts
        {
            int sourceArrayLength = array.Length;
            int firstArraySize = sourceArrayLength / 2;
            int secondArraySize = sourceArrayLength / 2 + sourceArrayLength % 2;
            int[] arr1 = new int[firstArraySize];
            int[] arr2 = new int[secondArraySize];
            Array.Copy(array, arr1, sourceArrayLength / 2);
            Array.Copy(array, firstArraySize, arr2, 0, secondArraySize);
            return new KeyValuePair<int[], int[]>(arr1, arr2);
        }

        private static int SimpleSum(int[] array) // SimpleSum of array
        {
            int sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }

        public static int FirstSum(int[] array) // Slicing array into 2 parts and parallel simple calculating
        {
            int firstSum = 0, secondSum = 0;
            KeyValuePair<int[], int[]> bisectedArray = BisectArray(array);
            ManualResetEvent firstSumIsReady = new ManualResetEvent(false);
            ManualResetEvent secondSumIsReady = new ManualResetEvent(false);
            Thread firstThread = new Thread(() =>
            {
                firstSum = SimpleSum(bisectedArray.Key);
                firstSumIsReady.Set();
            });
            Thread secondThread = new Thread(() =>
            {
                secondSum = SimpleSum(bisectedArray.Value);
                secondSumIsReady.Set();
            });
            firstThread.Start();
            secondThread.Start();
            firstSumIsReady.WaitOne();
            secondSumIsReady.WaitOne();
            return firstSum + secondSum;
        }

        public static int SecondSum(int[] array) // Recursion
        {
            if (array.Length == 1)
            {
                return array[0];
            }
            else
            {
                int firstSum = 0, secondSum = 0;
                ManualResetEvent firstSumIsReady = new ManualResetEvent(false);
                ManualResetEvent secondSumIsReady = new ManualResetEvent(false);
                KeyValuePair<int[], int[]> bisectedArray = BisectArray(array);
                Thread firstThread = new Thread(() =>
                {
                    firstSum = SecondSum(bisectedArray.Key);
                    firstSumIsReady.Set();
                });
                Thread secondThread = new Thread(() =>
                {
                    secondSum = SecondSum(bisectedArray.Value);
                    secondSumIsReady.Set();
                });
                firstThread.Start();
                secondThread.Start();
                firstSumIsReady.WaitOne();
                secondSumIsReady.WaitOne();
                return firstSum + secondSum;
            }
        }
    }
}
