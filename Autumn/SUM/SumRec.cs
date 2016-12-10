using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUM
{
    class SumRec : IArraySum
    {
        private const int _minLen = 32;

        public int Sum(int[] a)
        {
            if (a.Length < _minLen)                                     // if array is too short there is no profit in using that method
              return a.Sum();

            int[] Left  = new int[a.Length / 2];                        // split array on two and run Sum for each part
            int[] Right = new int[a.Length / 2 + a.Length % 2];

            Array.Copy(a, Left, Left.Length);
            Array.Copy(a, Left.Length, Right, 0, Right.Length);


            Task<int> lTask = Task.Run(() => Sum(Left));
            Task<int> rTask = Task.Run(() => Sum(Right));

            return lTask.Result + rTask.Result;
        }
    }
}
