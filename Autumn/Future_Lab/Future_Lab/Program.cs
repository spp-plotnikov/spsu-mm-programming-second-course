using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 10057;
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
                arr[i] = 1;
            Console.WriteLine("Size: " + size.ToString());
            Console.WriteLine("SegmentSum: " + new SegmentSum().Sum(arr).ToString());
            Console.WriteLine("RecursionSum: " + new RecursionSum().Sum(arr).ToString());
        }

    }
}
