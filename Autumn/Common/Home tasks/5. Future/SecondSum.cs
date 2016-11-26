using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesSum
{
    class SecondSum : IArraySum
    {
        public int Sum(int[] arr)
        {
            int size = arr.Count();
            if (size == 1)
            {
                return arr[0];
            }

            List<Task<int>> tasks = new List<Task<int>>();
            int sizeFirst = size / 2;
            int sizeSecond = size - sizeFirst;
            int[] tmpArrFirst = new int[sizeFirst];
            int[] tmpArrSecond = new int[sizeSecond];

            for (int i = 0; i < sizeFirst; i++)
            {
                tmpArrFirst[i] = arr[i];
            }
            tasks.Add(Task.Run(() =>
                      {
                          return new SecondSum().Sum(tmpArrFirst);
                      }));

            for (int i = sizeFirst; i < size; i++)
            {
                tmpArrSecond[i - sizeFirst] = arr[i];
            }
            tasks.Add(Task.Run(() =>
                     {
                         return new SecondSum().Sum(tmpArrSecond);
                     }));

            Task.WaitAll();
            int result = 0;
            for (int i = 0; i < tasks.Count(); i++)
            {
                result += tasks[i].Result;
            }
            return result;
        }
    }
}
