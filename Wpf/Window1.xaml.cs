using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfMathFormula
{
    /// <summary>
    /// Логика взаимодействия для Window.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int begin, end, curve;
        public Window1(int begin, int end, int curve)
        {
            InitializeComponent();
            this.curve = curve;
            this.begin = begin;
            this.end = end;

            DrawingGroup aDrawingGroup = new DrawingGroup();

            GeometryDrawing gr = new GeometryDrawing();
            GeometryGroup gg = new GeometryGroup();

            gr.Pen = new Pen(Brushes.Black, 1);
            gg = new GeometryGroup();

            double step = (Math.Abs(end) - begin);
            step = step / 10.0;
            Formula graf = new Formula();
            double y = 0;
            if (curve == 0) { y = graf.Hyperbola(begin, end, begin); }
            if (curve == 1) { y = graf.Cubic(begin, end, begin); }
            if (curve == 2) { y = graf.Oval(begin, end, begin); }

            Point last = new Point((int)((begin + 300)), (int)((y + 300)));
            for (double x = begin; x < end; x += step)
            {
                if (curve == 0) y = graf.Hyperbola(begin, end, x);
                if (curve == 1) { y = graf.Cubic(begin, end, x); }
                if (curve == 2) { y = graf.Oval(begin, end, x); }
                Point now = new Point((int)((x + 300)), (int)((y + 300)));
                LineGeometry l = new LineGeometry(now, last);
                gg.Children.Add(l);
                last = new Point((int)((x + 300)), (int)((y + 300))); ;
            }
            gr.Geometry = gg;
            aDrawingGroup.Children.Add(gr);
            image1.Source = new DrawingImage(aDrawingGroup);

        }

    }
}


