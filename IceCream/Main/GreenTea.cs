using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCream;

namespace IceCream
{
    
        public class GreenTea : IceCream.IceCreamMetod
        {
            public GreenTea()
            {
                packaging = "bowl";
                baseingredient = "green tea";
                type = "Ice cream from green tea";
                toping = "";
                form = "ball";
                kkal = 346;
                sprinklingConfectionery = "";
            }

            public override void Recipe()
            {
                Console.WriteLine(type + "\n");
                Console.Write("The milk is heated, preventing boiling. Lower in hot milk " + baseingredient +
                    "Cover and infuse.Pour into a container and freezeю Two hours later put in" + packaging + "in the form of" + form);
                if (toping != "") Console.WriteLine("Add" + toping);
                if (sprinklingConfectionery != "") Console.WriteLine("Add" + sprinklingConfectionery);
                Console.WriteLine("\n This will be " + kkal + " kkal");
            } // реализация абстрактного метода
        }
   
}
