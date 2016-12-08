using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Server;
using System.Threading.Tasks;

namespace Forms
{
    public partial class FiltersApp : Form
    {
        void GetFilters()
        {
 
            callback.RequestFilters();
            foreach (string filter in callback.Filters)
            {
                ListOfFilters.Items.Add(filter);
            }
        }
        private IService server;
        private bool cancel = false;
        private string currentFilter;
        private MyCallBack callback = new MyCallBack();
        public FiltersApp(IService Server)
        {
            this.server = Server;
            InitializeComponent();
            GetFilters();
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
            callback.Progress = 0;
            currentFilter = (string)ListOfFilters.SelectedItem;
        }


        private void Start_Click(object sender, EventArgs e)
        {
          
            Start.Enabled = false;
            ListOfFilters.Enabled = false;
            Load.Enabled = false;
            cancel = false;
            callback.RequestImage(currentFilter, (Bitmap)Source.BackgroundImage);
            while (callback.Progress != 100 && !cancel)
            {
                callback.RequestProgress();
                ProgressBar.Value = callback.Progress;
                Application.DoEvents();
            }
            if (!cancel)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap result = (Bitmap)tc.ConvertFrom(callback.Result);
                Target.BackgroundImage = result;
            }
            ProgressBar.Value = 0;
            callback.Progress = 0;
            Start.Enabled = true;
            ListOfFilters.Enabled = true;
            Load.Enabled = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            cancel = true;
            callback.Cancel();
        }
    }
}