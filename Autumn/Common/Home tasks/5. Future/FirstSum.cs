using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesSum
{
    class FirstSum : IArraySum
    {
        private readonly int taskNum = 500;
        public int Sum(int [] arr)
        {
            int size = arr.Count();
            int numElemInThread = (size / taskNum) + 1;

            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < taskNum; i++)
            {
                int tmpSize = Math.Max(Math.Min(numElemInThread, size - i * numElemInThread), 0);
                if (tmpSize == 0)
                {
                    break;
                }
                int[] tmpArr = new int[tmpSize];
                for (int j = 0; j < tmpSize; j++)
                {
                    tmpArr[j] = arr[i * numElemInThread + j];
                }
                tasks.Add(Task.Run(() => 
                         {
                             return tmpArr.Sum();
                         }));
            }

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
