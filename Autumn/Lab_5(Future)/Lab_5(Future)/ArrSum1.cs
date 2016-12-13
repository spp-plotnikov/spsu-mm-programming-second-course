using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_5_Future
{
    class ArrSum1 : IArraySum
    {
        public int Sum(int[] array)
        {
            int size = array.Count();

            if(size == 1)
                return array[0];

            List<Task<int>> tasks = new List<Task<int>>();
            for(int i = 0; i < 2; i++)
            {
                int[] arrInThread = new int[size / 2 + i * (size % 2)];
                Array.Copy(array, i * (size / 2), arrInThread, 0, size / 2 + i * (size % 2));
                tasks.Add(Task.Run(() =>
                     {
                         return Sum(arrInThread);
                     }));
            }
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