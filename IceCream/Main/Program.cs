using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icecreamprog
{

    class Program
    {
        static void Main(string[] args)
        {
            icecream.IceCream.greentea ice1 = new icecream.IceCream.greentea();
            icecream.IceCream.pistachio ice2 = new icecream.IceCream.pistachio();

            ice1.recipe();
            ice2.recipe(); 
        }
    }
}
