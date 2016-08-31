using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace MathLib
{
    public enum CurveType { Ellipse = 0, Parabola = 1 };

    public static class GraphBuilder
    {
        delegate DoublePoint[] PointListMaker(double start, double end, int scale);

        static PointListMaker[] pointListMaker = { Ellipse.GetPoints, Parabola.GetPoints };

        public static Point[] Draw(CurveType curveType, Info info)
        {

            Point initialPoint = new Point(info.Width / 2, info.Height / 2);

            Point[] pointList = MakePointList(curveType, initialPoint, info.Scale,
                -1 - (int)curveType * 3, 1 + (int)curveType * 3);

            return pointList;
        }

        private static Point[] MakePointList(CurveType curveType, Point initalPoint, int scale, double from, double to)
        {
            DoublePoint[] worklist = pointListMaker[(int)curveType](from, to, scale);
            List<Point> pointList = new List<Point>();

            foreach (DoublePoint dp in worklist)
            {
                Point point = new Point();
                point.X = (int)(dp.X * scale + initalPoint.X);
                point.Y = initalPoint.Y - (int)(dp.Y * scale);
                pointList.Add(point);
            }

            Point[] result = pointList.ToArray();

            return result;
        }
    }

    
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
