using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUM
{
    class SumLog : IArraySum
    {
        private const int _minLen = 64;
        public int Sum(int[] a)
        {
            if (a.Length < _minLen)                                     // if array is too short there is no profit in using that method
                return a.Sum();

            int len = (int)Math.Ceiling( Math.Log( a.Length, 2));       //Whole point is to split array on optimal size slises and sum 
                                                                        // it up
            
            Queue<Task<int>> res = new Queue<Task<int>>();

            int num = a.Length;
            int tmp = 0;
            for (int i = 0; i < len + 1; i++)
            {
                
                int t = Math.Min((int)Math.Floor(a.Length / (len * 1.0) ), num);
                if (t == (int)Math.Floor(a.Length / (len * 1.0)))       
                    num -= t;


                int[] arr = new int[t];
                
                Array.Copy(a,i* (i ==0 ? 0: tmp), arr, 0, t);
                res.Enqueue(Task.Run(() => (arr.Sum())));
                tmp= t;
            }

            int sum = 0;

            while (true)                                                 //wait untill all tasks will be done                                                
            {
                bool flag = true;
                foreach (Task<int> i in res)
                    if (i.Status != TaskStatus.RanToCompletion)
                    {
                        flag = false;
                    }
                if (flag)
                    break;
            }

            foreach (Task<int> i in res)
                sum += i.Result;

            return sum;
        }
    }
}
