using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    class Ellips
    {
        public static DoublePoint[] GetPoints(double from, double to, int scale)
        {
            List<DoublePoint> positiveList = new List<DoublePoint>();
            List<DoublePoint> negativeList = new List<DoublePoint>();
            double step = (to - from) / (double)(scale * 2);
            double curX = from;

            while (curX <= to)
            {
                DoublePoint point;
                point.X = curX;

                if ((1 - Math.Pow(curX, 2)) >= 0)
                {
                    point.Y = Math.Sqrt((1 - Math.Pow(curX, 2))) / 1.5;
                    positiveList.Add(point);

                    point.Y = -Math.Sqrt((1 - Math.Pow(curX, 2))) / 1.5;
                    negativeList.Add(point);
                }
                curX += step;
            }

            DoublePoint[] result = new DoublePoint[positiveList.Count * 2];

            positiveList.ToArray().CopyTo(result, 0);
            negativeList.Reverse();
            negativeList.ToArray().CopyTo(result, positiveList.Count);


            return result;
        }
    }
}
