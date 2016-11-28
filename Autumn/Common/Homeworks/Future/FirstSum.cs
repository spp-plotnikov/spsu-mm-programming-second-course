using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace future
{
    class FirstSum : IArraySum
    {
        // recursion
        public int Sum(int[] arr)
        {
            int size = arr.Length;
            if (size != 1)
            {
                List<Task<int>> tasks = new List<Task<int>>();
                Tuple<int[], int[]> intrArr = Slice(arr);

                // counting the first half
                tasks.Add(Task.Run(() =>
                    {
                        return Sum(intrArr.Item1);
                    }));

                // counting the second half
                tasks.Add(Task.Run(() =>
                    {
                        return Sum(intrArr.Item2);
                    }));

                Task.WaitAll();
                int finResult = 0;
                foreach (var task in tasks)
                {
                    finResult += task.Result;
                }

                return finResult;
            }
            else
            {
                return arr[0];
            }
        }

        private Tuple<int[], int[]> Slice(int[] arr)
        {
            int size = arr.Length;
            int[] arrSumFirst = new int[size / 2 + size % 2];
            Array.Copy(arr, 0, arrSumFirst, 0, size / 2 + size % 2);
            int[] arrSumSecond = new int[size / 2];
            Array.Copy(arr, size / 2 + size % 2, arrSumSecond, 0, size / 2);
            return new Tuple<int[], int[]>(arrSumFirst, arrSumSecond);
        }
    }
}
