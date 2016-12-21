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

namespace TestThreeOne
{
    public partial class TestThreeOne : Form
    {
        private List<long> _time;
        private List<string> _numOfGuys;
        public TestThreeOne(List<long> time, List<string> numOfClients)
        {
            InitializeComponent();
            _time = time;
            _numOfGuys = numOfClients;
        }

        private void TestThreeOne_Load(object sender, EventArgs e)
        {
            ThirdTest.ChartAreas[0].AxisX.Title = "Количество клиентов";
            ThirdTest.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            ThirdTest.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            ThirdTest.ChartAreas[0].AxisY.Interval = 5000;
            ThirdTest.ChartAreas[0].AxisY.Title = "Время, мс";
            ThirdTest.ChartAreas[0].AxisY.Maximum = _time.Max() + 2000;
            ThirdTest.ChartAreas[0].AxisY.Minimum = 0;
            Draw("время исполнения", _numOfGuys, _time);
            ThirdTest.SaveImage("ThirdTest.png", ChartImageFormat.Png);
        }

        private void Draw(string name, List<string> x, List<long> y)
        {
            var series = new Series(name);
            series.Points.DataBindXY(x, y);
            ThirdTest.Series.Add(series);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 3;
        }
    }
}
