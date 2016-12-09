using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystems
{
    class Bucket : ListItem
    {
        public int N;
        public Bucket(int n)
        {
            N = n;
            Bin = ToBinary(n);
            IsBucket = true;
        }
    }
}
