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

namespace Forms
{
    public partial class MyForm : Form
    {
       List<List<long>> points;
        public MyForm(List<List<long>> points)
        {
            InitializeComponent();
            this.points = points;
        }
        private void addLine(string name, List<string> x, List<long> points)
        {
            var series = new Series(name);
            series.Points.DataBindXY(x, points);
            chart.Series.Add(series);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var xInt = Enumerable.Range(1, points.Count()).ToList();
            var x = xInt.ConvertAll(r => r.ToString());
            chart.ChartAreas[0].AxisX.Title = "Number of clients";
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            chart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart.ChartAreas[0].AxisY.Interval = 150;
            chart.ChartAreas[0].AxisY.Title = "Time, ms";
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Near;
            chart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;

            var means = points.ConvertAll(r => r.Sum()/ r.Count());
            addLine("mean", x, means);
            var max = points.ConvertAll(r => r.Max());
            chart.ChartAreas[0].AxisY.Maximum = max.Max() + 100;
            addLine("max", x, max);
            var min = points.ConvertAll(r => r.Min());
            chart.ChartAreas[0].AxisY.Minimum = min.Min() - 150;
            var medians = points.ConvertAll(r =>
                {
                    var p = r;
                    p.Sort();
                    return p[p.Count() / 2];
                });
            addLine("median", x, medians);
            chart.SaveImage("result.png", ChartImageFormat.Png);
        }


    }
}
