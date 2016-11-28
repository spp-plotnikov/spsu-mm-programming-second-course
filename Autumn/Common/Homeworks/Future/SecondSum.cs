using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace future
{
    class SecondSum : IArraySum
    {
        public int IterativeSum(int[] arr)
        {
            int result = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                result += arr[i];
            }
            return result;
        }

        public int Sum(int[] arr)
        {
            int numOfParts = 1115;
            int size = arr.Length;
            List<Task<int>> tasks = new List<Task<int>>();

            if (numOfParts > size)
            {
                numOfParts = size;
            }

            int numInOne = size / numOfParts;
            int left = size % numOfParts;
            // for all parts but last
            for (int i = 0; i < numOfParts - 1; i++ )
            {
                int[] arrSum = new int[numInOne];
                Array.Copy(arr, i * numInOne, arrSum, 0, numInOne);
                tasks.Add(Task.Run(() =>
                {
                    return IterativeSum(arrSum);
                }));
            }

            // count for the last element
            int[] arrLast = new int[numInOne + left];
            Array.Copy(arr, (numOfParts - 1) * numInOne, arrLast, 0, numInOne + left);
            tasks.Add(Task.Run(() =>
            {
                return IterativeSum(arrLast);
            }));

            Task.WaitAll();
            int finResult = 0;
            foreach (var task in tasks)
            {
                finResult += task.Result;
            }

            return finResult;
        }
    }
}
