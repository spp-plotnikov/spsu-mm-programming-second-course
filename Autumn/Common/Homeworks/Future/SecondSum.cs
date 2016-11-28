using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace future
{
    class SecondSum : IArraySum
    {
        // very simple approach: divide the array in two parts and count the sum
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
            List<Task<int>> tasks = new List<Task<int>>();
            Tuple<int[], int[]> intrArr = RunTheProgramm.Slice(arr);

            // counting the first half
            tasks.Add(Task.Run(() =>
            {
                return IterativeSum(intrArr.Item1);
            }));

            // counting the second half
            tasks.Add(Task.Run(() =>
            {
                return IterativeSum(intrArr.Item2);
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
