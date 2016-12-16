using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientFilters
{
    public partial class Form1 : Form
    {
        private string path;
        private Bitmap curImage;
        private Filtering.ServiceClient host;
        ManualResetEvent progressFinished = new ManualResetEvent(true);
        ManualResetEvent cancelled = new ManualResetEvent(false);
        bool finished = true;

        public Form1()
        {
            InitializeComponent();
            this.host = new Filtering.ServiceClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images (*.bmp; *.jpg)|*.jpg; *.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            try
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                curImage = new Bitmap(pictureBox1.Image);
                path = openFileDialog1.FileName;
                textBox1.Text = path;
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendImage.Enabled = false;
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            BindingSource filters = new BindingSource();
            filters.DataSource = host.Filters();
            comboBox1.DataSource = filters;            
        }

        private void SendNGetImage(object sender, DoWorkEventArgs e)
        {
            bool error = false;
            this.Invoke((MethodInvoker)delegate ()
            {
                if (curImage == null) { MessageBox.Show("No image"); error = true; SendImage.Enabled = true; return; }
                if (!(comboBox1.Items.Count > 0)) { MessageBox.Show("Choose filter!"); error = true; SendImage.Enabled = true; return; }
            });
            if (error) { return; }
            finished = false;
            byte[] toSend = ConnectionMethods.ImageToByteArray(curImage);
            this.Invoke((MethodInvoker)delegate ()
            {
                host.SetFilter(comboBox1.SelectedValue.ToString());
            });
            if (!backgroundWorker2.IsBusy)
                backgroundWorker2.RunWorkerAsync();
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            host.DoFilter();
            byte[] result1 = ConnectionMethods.receiveByteArrayUsingChunks(host);
            pictureBox3.Image = ConnectionMethods.byteArrayToBitmap(result1);
            finished = true;
            this.Invoke((MethodInvoker)delegate ()
            {
                progressFinished.WaitOne();
                progressBar1.Value = 100;
                Thread.Sleep(100);
                label7.Text = "done";
                progressBar1.Value = 0;
                SendImage.Enabled = true;                
            });
        }

        private void UpdatingProgressBar(object sender, DoWorkEventArgs e)
        {
            progressFinished.Reset();
            this.Invoke((MethodInvoker)delegate ()
            {
                label7.Text = "sending";
            });
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            while (true)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    int progress = host.GetProgress();
                    if (Math.Abs(progressBar1.Value - progress) < 90)
                    {
                        if (progress < 99 && progress > 0)
                        {
                            progressBar1.Value = progress;
                            label7.Text = "filtering";
                        }
                        if (progress >= 99)
                        {
                            progressBar1.Value = progress;
                            label7.Text = "recieving";
                        }
                    }                    
                });
                Thread.Sleep(500);
                if (finished) { break; }
            }
            progressFinished.Set();      
        }               

        private void button4_Click(object sender, EventArgs e)
        {
            label7.Text = "cancelled";
            host.Cancel();
            ConnectionMethods.Cancelled = true;
        }
    }
}
