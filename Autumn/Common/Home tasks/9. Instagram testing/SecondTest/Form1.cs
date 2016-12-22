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

namespace SecondTest
{
    public partial class Form1 : Form
    {
        public Form1(List<int> mid, List<int> med, List<int> max, List<string> num)
        {
            InitializeComponent();
            Series("Average", num, mid);
            Series("Medium", num, med);
            Series("Maximum", num, max);
            chart1.SaveImage("SecondTest.png", ChartImageFormat.Png);
        }

        private void Series(string name, List<string> x, List<int> y)
        {
            Series series = new Series(name);
            series.Points.DataBindXY(x, y);
            chart1.Series.Add(series);
            series.BorderWidth = 3;
            series.ChartType = SeriesChartType.Line;
        }
    }
}
