using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Future
{
    class Program
    {

        static void Main(string[] args)
        {
            int[] arr = new int[3000];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }

            Console.WriteLine(new RecursiveSum().Sum(arr));
            Console.WriteLine(new FractionalSum().Sum(arr));
            Console.ReadKey();
        }
    }
}
