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
            
            byte[] toSend = ConnectionMethods.ImageToByteArray(curImage);
            host.SetByteArray(toSend.Length);
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            host.SetFilter(comboBox1.SelectedValue.ToString());
            host.DoFilter();
            byte[] result1 = ConnectionMethods.receiveByteArrayUsingChunks(host);
            
            pictureBox3.Image = ConnectionMethods.byteArrayToBitmap(result1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BindingSource filters = new BindingSource();
            filters.DataSource = host.Filters();
            comboBox1.DataSource = filters;            
        }
    }
}
