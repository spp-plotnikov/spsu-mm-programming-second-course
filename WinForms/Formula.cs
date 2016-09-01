using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathWinForm
{
    class Formula
    {
        public double Hyperbola(double begin, double end, double x)
        {
            double y = 5 * x * x + 8 * x + 11;
            return y;

        }
        public double Cubic(double begin, double end, double x)
        {
            double y = Math.Sqrt(Math.Abs(x) * x * x + 3 * x * x);
            return y;
        }
        public double Oval(double begin, double end, double x)
        {
            double c = 0.9;
            double y = Math.Sqrt(Math.Sqrt((1+4*c*c*x*x)-x*x-c*c));
            return y;
        }
       
    }
}
