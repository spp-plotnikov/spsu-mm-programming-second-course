using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCream
{
    public class Pistachio : IceCream.IceCreamMetod
    {
        public Pistachio()
        {
            packaging = "waffle cone";
            baseingredient = "crushed pistachio";
            type = "Pistachio ice cream with almonds";
            toping = "almonds";
            form = "soft";
            kkal = 359;
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
