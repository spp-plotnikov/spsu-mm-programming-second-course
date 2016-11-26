using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrSum
{
    class Summator1 : IArraySum
    {
        int numOfTasks = 5;      
        List<Task<int>> tasks = new List<Task<int>>();
        public int Sum(int[] a)
        {
            int delta = a.Length / (numOfTasks - 1);
            for (int i = 0; i < numOfTasks; i++)
            {
                
                if (i == numOfTasks - 1)
                {
                    int d = a.Length % (numOfTasks - 1);
                    int[] arHere = new int[d];

                    Array.Copy(a, i * delta, arHere, 0, d);
                    tasks.Add(Task.Run(() =>
                    {
                        return arHere.Sum();
                    }));
                }
                else
                {
                    int[] arHere = new int[delta];
                    Array.Copy(a, i * delta, arHere, 0, delta);
                    tasks.Add(Task.Run(() =>
                    {
                        return arHere.Sum();
                    }));
                }
                
            }

            int sum = 0;
            foreach (var t in tasks)
            {
                sum += t.Result;
            }
            return sum;
        }

       
    }
}
