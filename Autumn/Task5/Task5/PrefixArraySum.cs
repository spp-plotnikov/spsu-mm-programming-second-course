using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    class PrefixArraySum : IArraySum
    {
        public int Sum(int[] a)
        {
            return Prefix(a);    
        }
        private int Prefix(int[] a)
        {
            if(a.Length == 1)
            {
                return a[0];
            }
            else
            {
                List<Task<int>> tasks = new List<Task<int>>();
                if(a.Length % 2 != 0)
                {
                    a[a.Length - 2] += a[a.Length - 1];
                }
                for(int i = 0; i < a.Length / 2; i++)
                {
                    int j = i;
                    tasks.Add(Task.Run(() => a[2 * j] + a[2 * j + 1]));
                }
                Task.WaitAll(tasks.ToArray());
                List<int> acc = new List<int>();
                foreach(var task in tasks)
                {
                    acc.Add(task.Result);
                }
                return Prefix(acc.ToArray());
            }
        }
    }
}
