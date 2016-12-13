using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Server;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Forms
{
    public partial class FiltersApp : Form
    {

        private ProgressBarForm progressBarForm;
        private IService server;
        private bool cancel = false;
        private string currentFilter;
        private MyCallback callback;

        public FiltersApp(IService Server, MyCallback callback)
        {
            this.server = Server;
            this.callback = callback;
            InitializeComponent();
            GetFilters();
        }

        private void GetFilters()
        {
            foreach (string filter in server.ReadFilters())
            {
                ListOfFilters.Items.Add(filter);
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            callback.Progress = 0;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "png files (*.png)|*.png|bitmap files (*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                if (streamReader.BaseStream.Length > 1024 * 1024 * 10)
                {
                    MessageBox.Show("Size of file must be less than 10MB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bitmap sourceBitmap = (Bitmap)Image.FromStream(streamReader.BaseStream);
                    streamReader.Close();
                    Source.BackgroundImage = sourceBitmap;
                }
            }
            ListOfFilters.Enabled = true;
        }


        private void ListOfFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartButton.Enabled = true;
            callback.Progress = 0;
            currentFilter = (string)ListOfFilters.SelectedItem;
        }


        private void Start_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy != true)
            {
                progressBarForm = new ProgressBarForm();
                progressBarForm.Canceled += new EventHandler<EventArgs>(CancelButton_Click);
                progressBarForm.Show();
                this.Hide();
                backgroundWorker.RunWorkerAsync();
            }
            StartButton.Enabled = false;
            ListOfFilters.Enabled = false;
            LoadButton.Enabled = false;
            cancel = false;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] data;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                var image = Source.BackgroundImage;
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
            }
            server.SendFile(currentFilter, data);

            while (!callback.ImageHere && !cancel)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (!cancel)
                    {
                        worker.ReportProgress(callback.Progress);
                    }
                    Thread.Sleep(1000);
                }
            }
           
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            server.Cancel();
            cancel = true;
            backgroundWorker.CancelAsync();
            progressBarForm.Close();
            this.Show();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            progressBarForm.Close();
            this.Show();
            if (!cancel)
            {
                using (var ms = new MemoryStream(callback.Result))
                {
                    Image image = Image.FromStream(ms);
                    Target.BackgroundImage = image;
                }
            }
            callback.ImageHere = false;
            StartButton.Enabled = true;
            ListOfFilters.Enabled = true;
            LoadButton.Enabled = true;
            callback.Progress = 0;

        }


        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarForm.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            if(progressBarForm.ProgressValue <= e.ProgressPercentage)
                progressBarForm.ProgressValue = e.ProgressPercentage;
        }
    }
}