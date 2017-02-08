using SpersyService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SpersyApp
{
    public partial class Form1 : Form
    {
        private string currentFilter;
        private MyServiceCallback callback;
        private ServiceReference.ServiceClient client;
        private BackgroundWorker background;
        private bool proceeding;
        private bool isCanceled;
        private int percents;
        private Bitmap pic;

        public Form1()
        {
            InitializeComponent();
            callback = new MyServiceCallback();
            client = new ServiceReference.ServiceClient(new InstanceContext(callback), "NetTcpBinding_IService");
            background = new BackgroundWorker();
            background.WorkerSupportsCancellation = true;
            background.WorkerReportsProgress = true;
            background.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            background.DoWork += new DoWorkEventHandler(DoWork);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            currentFilter = null;
            percents = 0;
            isCanceled = false;
            proceeding = false;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Bitmap newImage = callback.Pic;
            if (isCanceled)
            {
                ProgressPB.Value = 100;
            }
            else
            {
                pic = newImage;
                ProgressPB.Value = 100;
            }
            ImagePicBox.Image = pic;
            proceeding = false;
            isCanceled = false;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            proceeding = true;
            ProgressPB.Value = 0;
            percents = 0;
            byte[] bytes = (byte[])(new ImageConverter().ConvertTo(pic, typeof(byte[])));
            client.ProceedImage(bytes, currentFilter);

            while (percents != 95)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    percents = callback.Percents;
                    worker.ReportProgress(percents);
                    Thread.Sleep(100);
                }
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressPB.Value = e.ProgressPercentage;
        }

        private void GetFiltersBtn_Click(object sender, EventArgs e)
        {
            FiltersComboBox.Items.Clear();
            FiltersComboBox.Items.AddRange(client.GetFilterNames());
        }

        private void SelectImageBtn_Click(object sender, EventArgs e)
        {
            if(proceeding)
            {
                return;
            }
            OpenFile.ShowDialog();
            ImagePicBox.Image = Image.FromFile(OpenFile.FileName);
            pic = (Bitmap)ImagePicBox.Image;
            ProgressPB.Value = 0;
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            if (proceeding)
            {
                return;
            }
            if (currentFilter != null && pic != null)
            {
                background.RunWorkerAsync();
            }
        }

        private void FiltersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(proceeding)
            {
                return;
            }
            currentFilter = FiltersComboBox.SelectedItem.ToString();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if(proceeding)
            {
                isCanceled = true;
                var stop = new Thread(() => { client.CancelProceeding();});
                stop.IsBackground = true;
                stop.Start();
            }
        }
    }
}
