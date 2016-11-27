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
using InstClient.Model;
using InstClient.ViewModel;

namespace InstClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var model = new ClientModel();
            var viewModel = new MainWindowViewModel(model);
            DataContext = viewModel;
            ResultImg.MouseLeftButtonDown += viewModel.ChangePictsVisibility;
            InitalImg.MouseLeftButtonUp += viewModel.ChangePictsVisibility;


            Closing += OnWindowClosing;
        }

        public void OnWindowClosing(object sender, EventArgs args)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}
