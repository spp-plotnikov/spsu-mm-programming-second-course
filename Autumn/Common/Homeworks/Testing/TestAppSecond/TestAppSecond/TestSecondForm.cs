using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestAppSecond
{
    public partial class TestSecondForm : Form
    {
        private List<long> _mid;
        private List<long> _max;
        private List<long> _medians;
        private List<string> _imagesSize;
        public TestSecondForm( List<long> mid, List<long> medians, List<long> max, List<string> imagesSize)
        {
            InitializeComponent();
            _mid = mid;
            _medians = medians;
            _max = max;
            _imagesSize = imagesSize;
        }

        private void TestSecondForm_Load(object sender, EventArgs e)
        {
            SecondTest.ChartAreas[0].AxisX.Title = "Количество пикселей";
            SecondTest.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            SecondTest.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            SecondTest.ChartAreas[0].AxisY.Interval = 1500;
            SecondTest.ChartAreas[0].AxisY.Title = "Время, мс";
            SecondTest.ChartAreas[0].AxisY.Maximum = _max.Max() + 100;
            SecondTest.ChartAreas[0].AxisY.Minimum = 0;
            Draw("Среднее", _imagesSize, _mid);
            Draw("Максимальное", _imagesSize, _max);
            Draw("Медиана", _imagesSize, _medians);
            SecondTest.SaveImage("SecondTest.png", ChartImageFormat.Png);
        }

        private void Draw(string name, List<string> x, List<long> y)
        {
            var series = new Series(name);
            series.Points.DataBindXY(x, y);
            SecondTest.Series.Add(series);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
        }
    }
}