using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace InstClient
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            var model = new ClientModel();
            var viewModel = new MainWindowViewModel(model);
            DataContext = viewModel;
            ResultImg.MouseLeftButtonDown += viewModel.ChangePictsVisibility;
            InitalImg.MouseLeftButtonUp += viewModel.ChangePictsVisibility;
            viewModel.OpenPictRequested += OpenPict;
            viewModel.SavePictRequested += SavePict;
            viewModel.ShowMessage += OnMessageShow;

            Closing += OnWindowClosing;
            Closing += viewModel.OnClosing;
        }

        public void OnWindowClosing(object sender, EventArgs args)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        public void OnMessageShow(object sender, ClientEventArgs args)
        {
            var title = args.Message.Split(' ')[0];
            var message = args.Message.Substring(args.Message.IndexOf(' '));
            MessageBox.Show(message, title);
        }

        public void OpenPict(object sender, EventArgs args)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Bitmap| *.bmp",
                CheckFileExists = true
            };
            if (dlg.ShowDialog() == true)
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null)
                    viewModel.InitalPictPath = dlg.FileName;
                //ssd
            }
        }

        public void SavePict(object sender, EventArgs args)
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    FileName = "temp",
                    DefaultExt = ".bmp",
                    Filter = "Bitmap(.bmp)|*.bmp"
                };
                if (dlg.ShowDialog() == true)
                {
                    var viewModel = DataContext as MainWindowViewModel;
                    File.Copy(viewModel.ResultPictPath, dlg.FileName, true);
                }
            }
            catch (Exception e)
            {

                OnMessageShow(null, new ClientEventArgs("Error" + e.ToString()));
            }
        }
    }
}
