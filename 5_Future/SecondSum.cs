using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class SecondSum : IArraySum
{
    public int Sum(int[] array)
    {
        int size = array.Length;
        if(size != 1)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < 2; i++)
            {
                int[] arrayInThread = new int[size / 2 + (size % 2) * i];
                Array.Copy(array, (size / 2) * i, arrayInThread, 0, size / 2 + (size % 2) * i);
                tasks.Add(Task.Run(() =>
                {
                    return Sum(arrayInThread);
                }
            ));
            }
            Task.WaitAll(tasks.ToArray());
            var result = new List<int>();
            foreach (var task in tasks)
            {
                result.Add(task.Result);
            }
            return result.Sum();
        }
        else
            return array[0];
    }
}
