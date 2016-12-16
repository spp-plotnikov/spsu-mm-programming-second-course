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
    public partial class ClientInterface : Form, MyServiceReference.IServiceCallback
    {
        List<RadioButton> radioButtonList = new List<RadioButton>();
        Bitmap imageBitmap;
        MyServiceReference.ServiceClient client;
        string filter;
        float prog = 0;
        bool workFlag = false;

        public ClientInterface()
        {
            var instanceContext = new InstanceContext(this);
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
            if (workFlag) return;
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
            if (imageBitmap == null)
            {
                MessageBox.Show("Null Picture");
                return;
            }

            filter = filt;
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(StartWork);
            backgroundWorker.RunWorkerAsync();
            SendPictureButton.Enabled = false;
            CancelWorkButton.Enabled = true;

        }

        private void CancelWorkButton_Click(object sender, EventArgs e)
        {
            if (!workFlag) return;
            client.StopWork();
            workFlag = false;
            backgroundWorker.CancelAsync();            
        }               

        private void StartWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker here = sender as BackgroundWorker;
            client.GetPicture(imageBitmap, filter);

            while(workFlag)
            {
                if (here.CancellationPending == true)
                {
                    e.Cancel = true;
                    workFlag = false;
                }                
            }
        }

        public void SendResult(byte[] res)
        {
            if (res == null)
            {
                progressBar.Value = 0;
                percent.Text = "Canceled";

            }
            else
            {
                newPictureBox.Image = ToBitMap(res);
                percent.Text = "Done";
            }
            SendPictureButton.Enabled = true;
            CancelWorkButton.Enabled = false;
            workFlag = false;
        }

        private Bitmap ToBitMap(byte[] array)
        {
            Bitmap res = new Bitmap(imageBitmap.Width, imageBitmap.Height);
            for (int i = 0; i < imageBitmap.Width; i++)
                for (int j = 0; j < imageBitmap.Height; j++)
                {
                    res.SetPixel(i, j, Color.FromArgb(array[i * imageBitmap.Height * 3 + j * 3], 
                                                      array[i * imageBitmap.Height * 3 + j * 3 + 1],
                                                      array[i * imageBitmap.Height * 3 + j * 3 + 2]));
                }
            return res;
        }

        public void SendProgress(float progr)
        {
            progressBar.Value = (int)progr;
            prog = progr;
            percent.Text = prog.ToString() + "%";
            if (prog == 100) percent.Text = "Waiting for a picture...";
        }       
    }
}
