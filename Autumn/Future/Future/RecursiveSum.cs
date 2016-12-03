using System.Collections.Generic;
using System.Threading.Tasks;

namespace Future
{
    public class RecursiveSum : IArraySum
    {
        public int Sum(int[] arr)
        {
            return PartialSum(arr, 0, arr.Length - 1).Result;
        }

        private async Task<int> PartialSum(int[] arr, int left, int right)
        {
            if (left == right)
            {
                return arr[left];
            }

            int m = (left + right) / 2;

            var leftTask = await Task.Run(() => PartialSum(arr, left, m));
            var rightTask = await Task.Run(() => PartialSum(arr, m + 1, right));

            return leftTask + rightTask;
        }
    }
}