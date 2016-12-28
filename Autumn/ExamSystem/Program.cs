using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            const int n = 10;
            Bots NaiveHAsh = new Bots(n, new NaiveOrganisation());
            Bots BucketHash = new Bots(n, new BOrganisation());

            Thread.Sleep(1000); 


            Console.Write("Naive Hаsh Organisation:  ");
            NaiveHAsh.StatInfo();
            Console.Write("Bucket Organisation:");
            BucketHash.StatInfo();
        }
    }
}
