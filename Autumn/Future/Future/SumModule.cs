using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future
{
    class SumModule: IArraySum
    {
        public int Sum(int[] arr)
        {
            Task<int> sumEven = Task<int>.Run(() =>
            {
                int result = 0;
                for (int i = 0; i < arr.Length; i += 2)
                    result += arr[i];
                return result;
            });

            Task<int> sumOdd = Task<int>.Run(() =>
            {
                int result = 0;
                for (int i = 1; i < arr.Length; i += 2)
                    result += arr[i];
                return result;
            });
            Task.WaitAll(sumEven, sumOdd);
            return sumEven.Result + sumOdd.Result;
        }
    }
}
