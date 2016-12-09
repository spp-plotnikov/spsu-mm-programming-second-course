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
                Bitmap sourceBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();
                Source.BackgroundImage = sourceBitmap;
             }
        }


        private void ListOfFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartButton.Enabled = true;
            callback.Progress = 0;
            currentFilter = (string)ListOfFilters.SelectedItem;
        }


        private void Start_Click(object sender, EventArgs e)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerAsync();

            CancelButton.Enabled = true;
            StartButton.Enabled = false;
            ListOfFilters.Enabled = false;
            LoadButton.Enabled = false;
            cancel = false;

            while (!callback.ImageHere && !cancel)
            {
                ProgressBar.Value = callback.Progress;
                Application.DoEvents();
            }
            if (!cancel)
            {
                using (var ms = new MemoryStream(callback.Result))
                {
                    Image image = Image.FromStream(ms);
                    Target.BackgroundImage = image;
                }
            }
            StartButton.Enabled = true;
            ListOfFilters.Enabled = true;
            LoadButton.Enabled = true;
            CancelButton.Enabled = true;
            ProgressBar.Value = 0;
            callback.Progress = 0;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            callback.ImageHere = false;
            server.SendFile(currentFilter, (Bitmap)Source.BackgroundImage);
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            cancel = true;
            server.Cancel();
        }
    }
}