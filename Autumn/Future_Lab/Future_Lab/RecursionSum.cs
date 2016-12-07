using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future_Lab
{
    class RecursionSum : IArraySum
    {
        private static int _maxSumLen = 50;
        public int Sum(int[] a)
        {
            if (a.Length <= _maxSumLen)
                return a.Sum();

            int[] aLeft = new int[a.Length / 2];
            int[] aRight = new int[a.Length / 2 + a.Length % 2];

            Array.Copy(a, aLeft, aLeft.Length);
            Array.Copy(a, aLeft.Length, aRight, 0, aRight.Length);

            Task<int> leftTask = Task.Run(() => Sum(aLeft));
            Task<int> rightTask = Task.Run(() => Sum(aRight));

            return leftTask.Result + rightTask.Result;
        }
    }
}
