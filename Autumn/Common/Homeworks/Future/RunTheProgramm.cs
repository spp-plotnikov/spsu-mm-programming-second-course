using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace future
{
    class RunTheProgramm
    {
        private int[] _arr;

        public RunTheProgramm(int[] arr)
        {
            _arr = arr;
        }

        public static Tuple<int[], int[]> Slice(int[] arr)
        {
            int size = arr.Length;
            int[] arrSumFirst = new int[size / 2 + size % 2];
            Array.Copy(arr, 0, arrSumFirst, 0, size / 2 + size % 2);
            int[] arrSumSecond = new int[size / 2];
            Array.Copy(arr, size / 2 + size % 2, arrSumSecond, 0, size / 2);
            return new Tuple<int[], int[]>(arrSumFirst, arrSumSecond);
        }

        public void Run()
        {
            FirstSum fi = new FirstSum();
            SecondSum se = new SecondSum();
            Console.WriteLine("The first sum is equal to {0}", fi.Sum(_arr));
            Console.WriteLine( "The second sum  is equal to {1}", fi.Sum(_arr), se.Sum(_arr));
        }
    }
}
