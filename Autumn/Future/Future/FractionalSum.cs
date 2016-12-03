using System;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class FractionalSum : IArraySum
    {
        public int Sum(int[] arr)
        {
            return SumTask(arr).Result;
        }

        private async Task<int> SumTask(int[] arr)
        {
            var taskCount = Math.Min(arr.Length, new Random().Next() % 100 + 200);
            var step = arr.Length/taskCount + 1;
            int result = 0;

            for (int i = 0; i < taskCount; i++)
            {
                var i1 = i;
                result += await Task.Run(() => SimpleSum(arr, i1*step, (i1 + 1)*step));
            }

            return result;
        }

        private int SimpleSum(int[] arr, int left, int right)
        {
            int result = 0;
            for (int i = left; i < Math.Min(arr.Length, right); i++) result += arr[i];
            return result;
        }
    }
}