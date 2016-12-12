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
            backgroundWorker1.DoWork += SendNGetImage;
            if (!backgroundWorker1.IsBusy && comboBox1.Items.Count > 0 && curImage != null)
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
            byte[] toSend = ConnectionMethods.ImageToByteArray(curImage);
            host.SetByteArray(toSend.Length);
            this.Invoke((MethodInvoker)delegate ()
            {
                host.SetFilter(comboBox1.SelectedValue.ToString());
            });
            backgroundWorker2.DoWork += UpdatingProgressBar;
            if (!backgroundWorker2.IsBusy)
                backgroundWorker2.RunWorkerAsync();
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            host.DoFilter();
            byte[] result1 = ConnectionMethods.receiveByteArrayUsingChunks(host);
            pictureBox3.Image = ConnectionMethods.byteArrayToBitmap(result1);
        }

        private void UpdatingProgressBar(object sender, DoWorkEventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            while (backgroundWorker1.IsBusy)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    int progress = host.GetProgress();
                    if (progress == 0)
                    {
                        label7.Text = "sending";
                    }
                    if (progress <= 100)
                    {
                        progressBar1.Value = progress;
                        label7.Text = "filtering";
                    }
                    if (progress == 100)
                    {
                        label7.Text = "recieving";
                    }
                });
                Thread.Sleep(1000);
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                progressBar1.Value = 0;
                label7.Text = "done";
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label7.Text = "cancelled";
        }
    }
}
