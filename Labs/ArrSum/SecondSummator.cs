using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrSum
{
    class SecondSummator : IArraySum
    {

        public int Sum(int[] a)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            if (a.Length == 1)
            {
                return a[0];
            }
            if (a.Length == 2)
            {
                return a[0] + a[1];
            }
            int[] lens = { a.Length / 2, a.Length / 2 + a.Length % 2 };
            int[] a1 = new int[lens[0]];
            int[] a2 = new int[lens[1]];
            Array.Copy(a, a1, lens[0]);
            Array.Copy(a, a2, lens[1]);

            tasks.Add(Task.Run(() =>
            {
                return Sum(a1);
            }));
            tasks.Add(Task.Run(() =>
            {
                return Sum(a2);
            }));

            int sum = 0;
            foreach (var t in tasks)
            {
                sum += t.Result;
            }
            return sum;
        }
    }
}
