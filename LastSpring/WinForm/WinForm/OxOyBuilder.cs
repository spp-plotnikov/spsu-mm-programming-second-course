using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinForm
{
    static class OxOyBuilder
    {
        public static void Build(Graphics graph, Point initalPoint, int scale)
        {
            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(0, initalPoint.Y),
                new Point(initalPoint.X * 2, initalPoint.Y));

            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(initalPoint.X, 0),
                new Point(initalPoint.X, initalPoint.Y * 2));

            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(initalPoint.X + scale, initalPoint.Y - 5),
                new Point(initalPoint.X + scale, initalPoint.Y + 5));

            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(initalPoint.X - scale, initalPoint.Y - 5),
                new Point(initalPoint.X - scale, initalPoint.Y + 5));

            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(initalPoint.X - 5, initalPoint.Y - scale),
                new Point(initalPoint.X + 5, initalPoint.Y - scale));

            graph.DrawLine(new Pen(Color.Black, 1),
                new Point(initalPoint.X - 5, initalPoint.Y + scale),
                new Point(initalPoint.X + 5, initalPoint.Y + scale));
        }
    }
}
