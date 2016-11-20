using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class FirstSum : IArraySum
{
    private int tasksNum = 1000;
    public int Sum(int[] array)
    {
        //distribute array
        int size = array.Length;
        int freeElems = size;
        int elemsInThread = size / tasksNum;
        int[] counts = new int[tasksNum];
        int[] indexes = new int[tasksNum];
        int count = 0;
        for (int i = 0; i < tasksNum - 1; i++)
        {
            indexes[i] = count;
            counts[i] = elemsInThread;
            count += elemsInThread;
            freeElems -= elemsInThread;
            elemsInThread = freeElems / (tasksNum - i - 1);
        }
        counts[tasksNum - 1] = freeElems;

        List<Task<int>> tasks = new List<Task<int>>();
        for (int i = 0; i < tasksNum; i++)
        {
            int[] arrayInThread = new int[counts[i]];
            Array.Copy(array, indexes[i], arrayInThread, 0, counts[i]);
            tasks.Add(Task.Run(() =>
            {
                return arrayInThread.Sum();
            }
        ));
        }

        Task.WaitAll(tasks.ToArray());
        var result = new List<int>();
        foreach (var task in tasks)
        {
            result.Add(task.Result);
        }
        return  result.Sum();
    }
}
