using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientInterface : Form//, MyServiceReference.IServiceCallback
    {
        List<RadioButton> radioButtonList = new List<RadioButton>();
        Bitmap imageBitmap;
        int width, height;
        MyServiceReference.ServiceClient client;
        string filter;
        float prog = 0;
        bool workFlag = false;
        CallbackFunctions callback;
        BackgroundWorker backgroundWorker;

        public ClientInterface(CallbackFunctions callback)
        {
            this.callback = callback;           
            var instanceContext = new InstanceContext(callback);
            client = new MyServiceReference.ServiceClient(instanceContext, "WSDualHttpBinding_IService");
            InitializeComponent();
            CreateFilterList();
            CancelWorkButton.Enabled = false;
        }

        private void CreateFilterList()
        {
            int x = 15;
            int y = 20;
            int j = 0;
            foreach (var f in client.GetFilters())
            {
                radioButtonList.Add(new RadioButton());
                radioButtonList[j].AutoSize = true;
                radioButtonList[j].Text = f;
                radioButtonList[j].Location = new Point(x, y);
                filterBox.Controls.Add(radioButtonList[j]);
                y += 25;
                j++;
            }
            radioButtonList[0].Checked = true;
        }

        private void AddPictureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image file (*.bmp,*.jpg)|*.bmp;*.jpg";
            if (file.ShowDialog() == DialogResult.OK)
            {
                imageBitmap = new Bitmap(file.FileName);
                this.oldPictureBox.Image = imageBitmap;                
            }
        }

        private void SendPictureButton_Click(object sender, EventArgs e)
        {
            callback.Progress = 0;
            progressBar.Value = 0;
            percent.Text = "0%";
            if (imageBitmap == null)
            {
                MessageBox.Show("Null Picture");
                return;
            }
            SendPictureButton.Enabled = false;
            height = imageBitmap.Height;
            width = imageBitmap.Width;
            workFlag = true;
            string filt = radioButtonList[0].Text;

            foreach (var but in radioButtonList)
            {
                if (but.Checked)
                {
                    filt = but.Text;
                    break;
                }
            }

            

            filter = filt;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(StartWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ProgBarWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FinishWork);
            backgroundWorker.RunWorkerAsync();           
            
            CancelWorkButton.Enabled = true;

        }

        private void StartWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker here = sender as BackgroundWorker;
            client.GetPicture(imageBitmap, filter);
            callback.IsHere = false;
            workFlag = true;

            while (workFlag && !callback.IsHere)
            {
                if (here.CancellationPending == true)
                {
                    e.Cancel = true;
                    workFlag = false;
                    break;
                }
                else
                {
                    backgroundWorker.ReportProgress((int)callback.Progress);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void ProgBarWork(object sender, ProgressChangedEventArgs e)
        {
            PrintProgress(e.ProgressPercentage);
        }

        private void FinishWork(object sender, RunWorkerCompletedEventArgs e)
        {
            if (callback.Result == null)
            {
                progressBar.Value = 0;
                percent.Text = "Canceled";
            }
            else
            {               
                using (var ms = new System.IO.MemoryStream(callback.Result))
                {

                    progressBar.Value = 100;
                    newPictureBox.Image = ToBitMap(callback.Result);
                    percent.Text = "Done";
                }
            }
            SendPictureButton.Enabled = true;
            CancelWorkButton.Enabled = false;
            AddPictureButton.Enabled = true;
            workFlag = false;
        }


        private void CancelWorkButton_Click(object sender, EventArgs e)
        {
            client.StopWork();
            workFlag = false;
            backgroundWorker.CancelAsync();
        }


        private Bitmap ToBitMap(byte[] array)
        {
            Bitmap res = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    res.SetPixel(i, j, Color.FromArgb(array[i * height * 3 + j * 3],
                                                      array[i * height * 3 + j * 3 + 1],
                                                      array[i * height * 3 + j * 3 + 2]));
                }
            return res;
        }

        public void PrintProgress(float progr)
        {
            progressBar.Value = (int)progr;
            prog = progr;
            percent.Text = prog.ToString() + "%";
            if (prog == 100)
            {
                progressBar.Value = 100;
                percent.Text = "Waiting for a picture...";              
                AddPictureButton.Enabled = false;
                CancelWorkButton.Enabled = false;
            }
        }      
    }
}
