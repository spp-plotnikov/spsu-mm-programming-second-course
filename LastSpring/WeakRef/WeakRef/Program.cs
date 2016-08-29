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
            int strongRefTime = 1000;
            var hashTab = new HashTable<int>(strongRefTime);
            for (int i = 1; i < 100; i++)
            {
                hashTab.Add(i);
            }

            Thread.Sleep(strongRefTime + 50);
            GC.Collect();

            for (int i = 1; i < 100; i++)
            {
                if(hashTab.Find(i))
                    Console.WriteLine("{0} Found", i);
                else
                    Console.WriteLine("{0} Not found", i);
            }
            Console.ReadLine();
        }
    }
}
