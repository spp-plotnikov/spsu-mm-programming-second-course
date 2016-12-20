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
        bool cancelled = false;
        bool finished = true;
        ManualResetEvent backgroundWorker1IsFree = new ManualResetEvent(true);

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
            backgroundWorker1IsFree.Reset();
            bool error = false;
            this.Invoke((MethodInvoker)delegate ()
            {
                if (curImage == null) { MessageBox.Show("No image"); error = true; SendImage.Enabled = true; return; }
                if (!(comboBox1.Items.Count > 0)) { MessageBox.Show("Choose filter!"); error = true; SendImage.Enabled = true; return; }
            });
            if (error) { backgroundWorker1IsFree.Set(); return; }
            cancelled = false;
            ConnectionMethods.Cancelled = false;            
            finished = false;
            byte[] toSend = ConnectionMethods.ImageToByteArray(curImage);
            this.Invoke((MethodInvoker)delegate ()
            {
                host.SetFilter(comboBox1.SelectedValue.ToString());
            });
            if (!backgroundWorker2.IsBusy) { backgroundWorker2.RunWorkerAsync(); }
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            host.DoFilter();
            byte[] result1 = ConnectionMethods.receiveByteArrayUsingChunks(host);
            if (!cancelled)
            {                
                pictureBox3.Image = ConnectionMethods.byteArrayToBitmap(result1);
            }
            else
            {
                backgroundWorker1IsFree.Set();
                return;
            }
            finished = true;
            this.Invoke((MethodInvoker)delegate ()
            {
                progressFinished.WaitOne();
                progressBar1.Value = 100;
                label7.Text = "done";
                progressBar1.Value = 0;
                SendImage.Enabled = true;
                backgroundWorker1IsFree.Set();            
            });
        }

        private void UpdatingProgressBar(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            progressFinished.Reset();
            this.Invoke((MethodInvoker)delegate ()
            {
                label7.Text = "sending";
            });
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            while (!cancelled)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    progress = host.GetProgress();
                    if (Math.Abs(progressBar1.Value - progress) < 90)
                    {
                        if (progress < 99 && progress > 0)
                        {
                            progressBar1.Value = progress;
                            label7.Text = "filtering";
                        }
                        if (progress >= 98)
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
            label7.Text = "cancellation";
            cancelled = true;
            host.Cancel();
            ConnectionMethods.Cancelled = true;
            this.Invoke((MethodInvoker)delegate ()
            {
                progressBar1.Value = 0;
                backgroundWorker1IsFree.WaitOne();
                label7.Text = "cancelled";
                SendImage.Enabled = true;
            });
        }
    }
}
