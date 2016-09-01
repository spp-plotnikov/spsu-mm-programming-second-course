using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icecream
{
    class IceCream
    {
        public abstract class icecream
        {
            public string packaging;
            public string type;
            public string baseingredient;
            public string toping;
            public string form;
            public double kkal;
            public string sprinklingConfectionery;

            public abstract void recipe();

        }

        public class greentea : icecream
        {
            public greentea()
            {
                packaging = "bowl";
                baseingredient = "green tea";
                type = "Ice cream from green tea";
                toping = "";
                form = "ball";
                kkal = 346;
                sprinklingConfectionery = "";
            }
            public override void recipe()
            {
                Console.WriteLine(type + "\n");
                Console.Write("The milk is heated, preventing boiling. Lower in hot milk " + baseingredient +
                    "Cover and infuse.Pour into a container and freezeю Two hours later put in" + packaging + "in the form of" + form);
                if (toping != "") Console.WriteLine("Add" + toping);
                if (sprinklingConfectionery != "") Console.WriteLine("Add" + sprinklingConfectionery);
                Console.WriteLine("\n This will be " + kkal + " kkal");
            } // реализация абстрактного метода
        }

        public class pistachio : icecream
        {
            public pistachio()
            {
                packaging = "waffle cone";
                baseingredient = "crushed pistachio";
                type = "Pistachio ice cream with almonds";
                toping = "almonds";
                form = "soft";
                kkal = 359;
                sprinklingConfectionery = "";
            }
            public override void recipe()
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
}
