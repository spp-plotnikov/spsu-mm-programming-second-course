using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibrary
{
    public struct Pixel
    {
        public int Red;
        public int Green;
        public int Blue;

        public static Pixel operator *(Pixel p, int i)
        {
            Pixel result;
            result.Red = p.Red * i;
            result.Green = p.Green * i;
            result.Blue = p.Blue * i;

            return result;
        }

        public static Pixel operator /(Pixel p, int i)
        {
            Pixel result;
            result.Red = p.Red / i;
            result.Green = p.Green / i;
            result.Blue = p.Blue / i;

            return result;
        }

        public static Pixel operator +(Pixel p1, Pixel p2)
        {
            Pixel result;
            result.Red = (p1.Red + p2.Red);
            result.Green = (p1.Green + p2.Green);
            result.Blue = (p1.Blue + p2.Blue);

            return result;
        }

        public static implicit operator Pixel(int v)
        {
            Pixel p;
            p.Red = v;
            p.Green = v;
            p.Blue = v;
            return p;
        }
    }
}
