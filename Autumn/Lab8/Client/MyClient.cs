using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class MyClient : Form
    {
        Bitmap _image;
        string[] _filters;
        string _filter;
        bool _inProcess;
        int _curProgress;
        bool _cancelProcess;

        BackgroundWorker _worker;

        ClientConnection _callbackClient;
        ServiceReference.ServiceClient _server;

        public MyClient(ClientConnection callback)
        {
            InitializeComponent();

            _callbackClient = callback;
            _server = new ServiceReference.ServiceClient(new InstanceContext(_callbackClient), "NetTcpBinding_IService");

            _cancelProcess = false;
            _curProgress = 0;
            _inProcess = false;
            _filters = _server.GetFilters();
            foreach (var filter in _filters)
            {
                filterSelectionBox.Items.Add(filter);
            }

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += new DoWorkEventHandler(DoWork);
            _worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
        }

        private void ChooseImage(object sender, EventArgs e)
        {
            if (!_inProcess)
            {
                using (OpenFileDialog openImage = new OpenFileDialog())
                {
                    openImage.Filter = "bmp files (*.bmp)|*.bmp";
                    openImage.Title = "Open image";
                    openImage.InitialDirectory = @"C:\";

                    if (openImage.ShowDialog() == DialogResult.OK)
                    {
                        _image = new Bitmap(openImage.FileName);
                        imageBox.Load(openImage.FileName);
                    }
                    progressBar.Value = 0;
                }
            }
        }

        private void FilterSelected(object sender, EventArgs e)
        {
            if (!_inProcess)
            {
                _filter = filterSelectionBox.SelectedItem.ToString();
            }
        }

        private void SendImageToServer(object sender, EventArgs e)
        {
            if (!_inProcess)
            {
                if (_filter != null && _image != null)
                {
                    _worker.RunWorkerAsync();
                }
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (!_inProcess)
            {
                if (imageBox.Image != null)
                {
                    using (SaveFileDialog saveImage = new SaveFileDialog())
                    {
                        saveImage.Filter = "bmp files (*.bmp)|*.bmp";
                        saveImage.Title = "Save image";
                        saveImage.FileName = "*.bmp";
                        saveImage.RestoreDirectory = true;

                        if (saveImage.ShowDialog() == DialogResult.OK && saveImage.FileName != "")
                        {
                            FileStream saveStream = (FileStream)saveImage.OpenFile();
                            _image.Save(saveStream, ImageFormat.Bmp);
                            saveStream.Close();
                            imageBox.Image = null;
                        }
                    }
                }
            }
        }

        private void CancelProcessingButtonClick(object sender, EventArgs e)
        {
            if (_inProcess)
            {
                _cancelProcess = true;
                Thread cancel = new Thread(() => { _server.SendCancel(true); });
                cancel.IsBackground = true;
                cancel.Start();
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            _inProcess = true;
            progressBar.Invoke(new Action(() => { progressBar.Value = 0; }));
            _curProgress = 0;
            ImageConverter converter = new ImageConverter();
            byte[] imageArray = (byte[])converter.ConvertTo(_image, typeof(byte[]));
            _server.SendImage(imageArray, _filter);

            while (_curProgress != 75)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    _curProgress = _callbackClient.Progress;
                    worker.ReportProgress(_curProgress);
                    Thread.Sleep(100);
                }
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Bitmap newImage = _callbackClient.Image;
            if (!_cancelProcess)
            {
                _image = newImage;
                progressBar.Invoke(new Action(() => { progressBar.Value = 100; }));
            }
            else
            {
                progressBar.Invoke(new Action(() => { progressBar.Value = 0; }));
            }
            imageBox.Invoke(new Action(() => { imageBox.Image = _image; }));
            _inProcess = false;
            _cancelProcess = false;
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Invoke(new Action(() => { progressBar.Value = e.ProgressPercentage; }));
        }
    }
}

