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
using InstServer.Model;
using InstServer.ViewModel;

namespace InstServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel _model;

        public MainWindow()
        {
            InitializeComponent();
            var model = new MainWindowModel();
            _model = model;

            var viewModel = new MainWindowViewModel(model);
            DataContext = viewModel;
            Closing += viewModel.OnWindowClosing;
        }

    }
}
