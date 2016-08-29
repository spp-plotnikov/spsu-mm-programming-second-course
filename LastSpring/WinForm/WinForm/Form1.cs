using MathLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            string graph = (string)GraphListCmb.SelectedItem;

            Point initalPoint = new Point(GrapBoxPctb.Width / 2, GrapBoxPctb.Height / 2);

            Info info = new Info(GrapBoxPctb.Width, GrapBoxPctb.Height, ScaleBar.Value);

            CurveType curveType;

            if (Enum.TryParse(graph, out curveType))
            {
                GrapBoxPctb.Refresh();

                OxOyBuilder.Build(GrapBoxPctb.CreateGraphics(), initalPoint, ScaleBar.Value);

                GrapBoxPctb.CreateGraphics().DrawCurve(new Pen(Color.Black, 1), GraphBuilder.Draw(curveType, info));
            }
            



        }

        

        private void ScaleBar_ValueChanged(object sender, EventArgs e)
        {
            GrapBoxPctb.Refresh();
            PrintBtn_Click(null, null);
        }
    }
}
