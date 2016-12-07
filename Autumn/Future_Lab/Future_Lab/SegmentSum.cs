using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Future_Lab
{
    class SegmentSum : IArraySum
    {
        private static int _numOfTasks = 200;
        public int Sum(int[] a)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            int res = 0;
            int averageLen = a.Length / _numOfTasks;
            int curLen;
            for (int i = 0; i < _numOfTasks; i++)
            {
                if (i == _numOfTasks-1)
                    curLen = averageLen + a.Length % _numOfTasks;
                else
                    curLen = averageLen;
                int[] arHere = new int[curLen];
                Array.Copy(a, i * averageLen, arHere, 0, curLen);
                tasks.Add(Task.Run(() => arHere.Sum()));
            }

            foreach (Task<int> task in tasks)
                res += task.Result;

            return res;
        }
    }
}
