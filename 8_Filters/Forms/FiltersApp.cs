using System;
using Contract;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.ServiceModel;
using System.IO;
using BitmapFilters;
using System.Threading;

namespace Forms
{
    public partial class FiltersApp : Form
    {
        private bool cancel = false;
        private string currentFilter;
        private IMyService client;
        public FiltersApp()
        {
            ChannelFactory<IMyService> factory;
            factory = new ChannelFactory<IMyService>(
                new BasicHttpBinding(), "http://localhost:81");
            this.client = factory.CreateChannel();
            InitializeComponent();

            //Load filters
            var filters = client.GetFilters("Filters.txt");
            foreach (string filter in filters)
                ListOfFilters.Items.Add(filter);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png files (*.png)|*.png|Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpg)|*.jpg";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                Bitmap sourceBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();
                Source.BackgroundImage = sourceBitmap;
                Target.BackgroundImage = sourceBitmap;
            }
        }

        private void ListOfFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFilter = (string)ListOfFilters.SelectedItem;
        }


        private void Start_Click(object sender, EventArgs e)
        {

            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            BackgroundWorker.RunWorkerAsync();
            Start.Enabled = false;
            ListOfFilters.Enabled = false;
            Load.Enabled = false;
            cancel = false;
            while (BackgroundWorker != null && BackgroundWorker.IsBusy)
            {
                Thread.Sleep(1000);
                if(ProgressBar.Value != 90)
                {
                    ProgressBar.Increment(10);
                }
                Application.DoEvents();
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap result = null;
            switch (currentFilter)
            {
                case "Grayscale":
                    result = Source.BackgroundImage.Grayscale();
                    break;
                case "Negative":
                    result = Source.BackgroundImage.Negative();
                    break;
                case "SepiaTone":
                    result = Source.BackgroundImage.SepiaTone();
                    break;
                case "Transparency":
                    result = Source.BackgroundImage.Transparency();
                    break;
            }
            if (!cancel)
            {
                Target.BackgroundImage = result;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!cancel)
            {
                ProgressBar.Value = 100;
            }
            Start.Enabled = true;
            ListOfFilters.Enabled = true;
            Load.Enabled = true;
            ProgressBar.Value = 0;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker.WorkerSupportsCancellation = true;
            BackgroundWorker.CancelAsync();
            BackgroundWorker.Dispose();
            BackgroundWorker = null;
            cancel = true;
            ProgressBar.Value = 0;
        }
    }
}