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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathLib;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GraphViewModel graphViewModel;

        public MainWindow()
        {
            InitializeComponent();
            graphViewModel = new GraphViewModel();
            graphViewModel.mw = this;
            imgGraph.DataContext = graphViewModel;
            ScaleBar.DataContext = graphViewModel;
        }

        public void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string graph = null; 

            if ((TextBlock) GraphListCmb.SelectedItem != null)
            {
                graph = ((TextBlock)GraphListCmb.SelectedItem).Text;
                Point initalPoint = new Point(imgGraph.Width / 2, imgGraph.Height / 2);

                Info info = new Info((int)imgGraph.Width, (int)imgGraph.Height, (int)ScaleBar.Value);

                CurveType curveType;

                if (Enum.TryParse(graph, out curveType))
                {
                    System.Drawing.Point[] points = GraphBuilder.Draw(curveType, info);

                    graphViewModel.Repaint(points, imgGraph.Height, imgGraph.Width, info.Scale);
                }
            }
        }
    }
}
