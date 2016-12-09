using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            // example array
            int[] arr = new int[100];
            for (int i = 0; i < 50; i++)
                arr[i] = 1;

            // summing class
            //AsyncArraySum adder = new AsyncArraySum(SumImplementation.ThreadPoolSum);
            AsyncArraySum adder = new AsyncArraySum(SumImplementation.RecursiveSum); // more then 1000 - out of memory

            // start summing 


            // for test 
            Console.WriteLine("Waiting main thread tasks");
            //Thread.Sleep(5000);
            Console.WriteLine("Done main thread tasks, waiting summing...");

            // getting sum, if sum was not calc yet - waiting
            int sum = adder.Sum(arr);

            Console.WriteLine("Sum of array: " + sum);

            Console.ReadKey();
        }
    }
}
