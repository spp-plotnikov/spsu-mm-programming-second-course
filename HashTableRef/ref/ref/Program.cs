using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hashTableRef;
using System.Threading;

namespace @ref
{
    class Program
    {
        static void Main(string[] args)
        {
            int time = 10000;
            var table = new HashTable<int>(time);

            for (int i = 1; i < 10; i++)
                table.Add(i);

            for (int i = 1; i < 10; i++)
                table.Find(i);

            Thread.Sleep(time + 10);

            GC.Collect();

            for (int i = 1; i < 10; i++)
                table.Find(i);

        }

    }
}
