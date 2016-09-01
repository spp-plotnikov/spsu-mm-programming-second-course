using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{

    public class Info
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Scale { get; private set; }

        public Info(int width, int height, int scale)
        {
            Width = width;
            Height = height;
            Scale = scale;
        }
    }
}
