﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace future
{
    class FirstSum : IArraySum
    {
        // rucursion
        public int Sum(int[] arr)
        {
            int size = arr.Length;
            if (size != 1)
            {
                List<Task<int>> tasks = new List<Task<int>>();
                Tuple<int[], int[]> intrArr = RunTheProgramm.Slice(arr);

                // counting the first half
                tasks.Add(Task.Run(() =>
                    {
                        return Sum(intrArr.Item1);
                    }));

                // counting the second half
                tasks.Add(Task.Run(() =>
                    {
                        return Sum(intrArr.Item2);
                    }));

                Task.WaitAll();
                int finResult = 0;
                foreach (var task in tasks)
                {
                    finResult += task.Result;
                }

                return finResult;
            }
            else
            {
                return arr[0];
            }
        }

    }
}
