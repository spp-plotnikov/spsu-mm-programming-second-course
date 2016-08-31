using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    class Parabola
    {
        public static DoublePoint[] GetPoints(double from, double to, int scale)
        {
            List<DoublePoint> positiveList = new List<DoublePoint>();
            double step = (to - from) / (double)(scale * 2);
            double curY = from;

            while (curY <= to)
            {
                DoublePoint point;
                point.Y = curY;

                point.X = Math.Pow(curY, 2);
                positiveList.Add(point);

                curY += step;
            }

            DoublePoint[] result = positiveList.ToArray();

            return result;
        }
    }
}
