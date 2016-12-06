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
        public int n;
        public Bucket(int n)
        {
            this.n = n;
            Bin = toBinary(n);
            IsBucket = true;
        }
    }
}
