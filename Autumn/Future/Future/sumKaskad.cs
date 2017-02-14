using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future
{
    class SumKaskad: IArraySum
    {

        public int Sum(int[] arr)
        {
            if (arr.Length == 1)
                return arr[0];
            int[] otherArr = new int[arr.Length / 2];
            Task<int>[] tasks = new Task<int>[arr.Length / 2];
            for (int i = 0; i < arr.Length / 2; i++)
            {
                tasks[i] = Task<int>.Run(() => arr[2 * i] + arr[2 * i + 1]);
            }
            for (int i = 0; i<arr.Length / 2; i++)
            {
                otherArr[i]= tasks[i].Result;
            }
            if (arr.Length % 2 == 0)
                return Sum(otherArr);
            else
                return Sum(otherArr) + arr[arr.Length - 1];
        }
    }
}
