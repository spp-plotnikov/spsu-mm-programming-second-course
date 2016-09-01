using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathLib;

namespace WPF
{
    

    public class GraphViewModel : INotifyPropertyChanged
    {
        private DrawingImage _source = new DrawingImage();
        private GeometryGroup _geometryGroup = new GeometryGroup();
        public MainWindow mw;

        public event PropertyChangedEventHandler PropertyChanged;

        public DrawingImage ImgSource
        {
            get
            {
                return _source;
            }
        }

        public double IsValueChanged
        {
            private get
            {
                return 0;
            }
            set
            {
                mw.btnPrint_Click(null, null);
            }
        } 

        public void Repaint(System.Drawing.Point[] points, double h, double w, int scale)
        {
            GeometryDrawing source = new GeometryDrawing();

            _geometryGroup.Children.Clear();

            for (int i = 0; i < points.Length - 1; i++)
            {
                LineGeometry line = new LineGeometry(new Point((double)points[i].X, (double)points[i].Y),
                    new Point((double)points[i + 1].X, (double)points[i + 1].Y));

                if (line.StartPoint.Y > 0 && line.StartPoint.Y < h &&
                    line.EndPoint.Y > 0 && line.EndPoint.Y < h)
                {
                    _geometryGroup.Children.Add(line);
                }

            }

            OxOyBuilder(h, w, scale);

            source.Geometry = _geometryGroup;

            source.Pen = new Pen(Brushes.Black, 1);

            _source = new DrawingImage(source);

            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ImgSource"));
            }
        }

        private void OxOyBuilder(double height, double width, int scale)
        {
            LineGeometry Ox = new LineGeometry(new Point(0, height / 2),
                    new Point(width, height / 2));
            _geometryGroup.Children.Add(Ox);

            LineGeometry Oy = new LineGeometry(new Point(width / 2, 0),
                    new Point(width / 2, height));
            _geometryGroup.Children.Add(Oy);

            LineGeometry One;

            One = new LineGeometry(new Point(width / 2 - 10, height / 2 + scale), new Point(width / 2 + 10, height / 2 + scale));
            _geometryGroup.Children.Add(One);
            One = new LineGeometry(new Point(width / 2 - 10, height / 2 - scale), new Point(width / 2 + 10, height / 2 - scale));
            _geometryGroup.Children.Add(One);
            One = new LineGeometry(new Point(width / 2 - scale, height / 2 - 10), new Point(width / 2 - scale, height / 2 + 10));
            _geometryGroup.Children.Add(One);
            One = new LineGeometry(new Point(width / 2 + scale, height / 2 - 10), new Point(width / 2 + scale, height / 2 + 10));
            _geometryGroup.Children.Add(One);

        }
    }
}
