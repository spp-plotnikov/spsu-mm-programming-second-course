using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_5_Future
{
    public class ArrSum2 : IArraySum
    {
        int taskNum = 100;
        public int Sum(int[] arr)
        {
            int size = arr.Count();
            if(taskNum > size)
            {
                taskNum = size;
            }
            int numElemInThread = size / taskNum;

            List<Task<int>> tasks = new List<Task<int>>();
            for(int i = 0; i < taskNum - 1; i++)
            {
                int[] tmpArr = new int[numElemInThread];
                for(int j = 0; j < numElemInThread; j++)
                {
                    tmpArr[j] = arr[i * numElemInThread + j];
                }
                tasks.Add(Task.Run(() =>
                    {
                        return tmpArr.Sum();
                    }));
            }

            int[] lastArr = new int[numElemInThread + size % taskNum];
            for(int j = 0; j < numElemInThread + size % taskNum; j++)
            {
                lastArr[j] = arr[numElemInThread * (taskNum - 1) + j];
            }
            tasks.Add(Task.Run(() =>
                {
                    return lastArr.Sum();
                }));
            Task.WaitAll(tasks.ToArray());

            int result = 0;
            foreach(var task in tasks)
            {
                result += task.Result;
            }
            return result;
        }
    }
}