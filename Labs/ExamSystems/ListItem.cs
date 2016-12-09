using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystems
{
    class ListItem
    {
        public Mutex BucketMutex = new Mutex();
        public ListItem Next = null;
        public string Bin;
        public bool IsBucket = false;

        protected string ToBinary(long number)
        {
            long n = number;
            string s = "";
            int k = 16;
            while (n != 0)
            {
                s += Convert.ToString(n % 2);
                n = n / 2;
                k--;
            }
            while (k > 0)
            {
                s += "0";
                k--;
            }
            return s;
        }
    }
}
