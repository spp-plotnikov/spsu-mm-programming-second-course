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

        public void Run()
        {
            FirstSum fi = new FirstSum();
            SecondSum se = new SecondSum();
            Console.WriteLine("The first sum is equal to {0}", fi.Sum(_arr));
            Console.WriteLine( "The second sum  is equal to {1}", fi.Sum(_arr), se.Sum(_arr));
        }
    }
}
