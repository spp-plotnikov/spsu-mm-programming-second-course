using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamProg
{

    class Program
    {
        static void Main(string[] args)
        {
            IceCream.GreenTea ice1 = new IceCream.GreenTea();
            IceCream.IceCream.Pistachio ice2 = new IceCream.IceCream.Pistachio();

            ice1.Recipe();
            ice2.Recipe(); 
        }
    }
}
