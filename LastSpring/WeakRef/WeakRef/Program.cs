using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeakRef
{
    class Program
    {
        static void Main(string[] args)
        {
            int StrongRefTime = 1000;
            var hashTab = new HashTable<int>(StrongRefTime);
            for (int i = 1; i < 100; i++)
            {
                hashTab.Add(i);
            }

            Thread.Sleep(StrongRefTime + 50);
            GC.Collect();

            for (int i = 1; i < 100; i++)
            {
                hashTab.Find(i);
            }
            Console.ReadLine();
        }
    }
}
