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
using System.IO;
using Server;

namespace Program
{
    public partial class FilterForm : Form
    {
        /*
         * Fields
         * */

        private int curFilter;
        private IService serviceServer;
        private Thread filtering;
        private Thread proccessing;
        private Bitmap curImage;
        private bool isWorking = false;

        /*
         * Additional functions
         * */

        delegate void Procceed();

        private Bitmap FromByteToBitmap(byte[] array)
        {
            Bitmap image = new Bitmap(curImage.Width, curImage.Height);
            if (isWorking)
            {
                for (int i = 0; i < curImage.Width; i++)
                {
                    for (int j = 0; j < curImage.Height; j++)
                    {
                        int R = (int)array[i * curImage.Height * 3 + j * 3 + 0];
                        int G = (int)array[i * curImage.Height * 3 + j * 3 + 1];
                        int B = (int)array[i * curImage.Height * 3 + j * 3 + 2];
                        image.SetPixel(i, j, Color.FromArgb(R, G, B));
                    }
                }
            }
            return image;
        }

        private void GetFilters()
        {
            string[] filters = serviceServer.GetFilters();
            for (int i = 0; i < filters.Length; i++)
            {
                filterBox.Items.Add(filters[i]);
            }
        }

        private void SetProgressBar(int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Procceed(delegate () { progressBar.Value = progress; }));
            }
            else
            {
                progressBar.Value = progress;
            }
        }

        private void Clear()
        {
            serviceServer.Clear();
            progressBar.Value = 0;
            Deactiv();
        }

        private void Activ()
        {
            filterButton.Enabled = true;
            toolStripMenuItem1.Enabled = true;
        }

        private void Deactiv()
        {
            filterButton.Enabled = false;
            toolStripMenuItem1.Enabled = false;
        }

        private void Run()
        {
            filtering = new Thread(() => { serviceServer.Filter(curImage, curFilter); });
            filtering.IsBackground = true;
            filtering.Start();

            while (progressBar.Value != 100 && isWorking)
            {
                SetProgressBar(serviceServer.GetProgress());
                Thread.Sleep(3);
            }

            if (isWorking)
            {
                curImage = FromByteToBitmap(serviceServer.GetImage());
            }
            else
            {
                SetProgressBar(0);
            }

            if (InvokeRequired)
            {
                Invoke(new Procceed(delegate () { pictureBox.Image = curImage; Activ(); }));
            }
            else
            {
                pictureBox.Image = curImage;
                Activ();
            }

            isWorking = false;
        }

        /*
         * Form functions
         * */

        public FilterForm(IService server)
        {
            serviceServer = server;
            InitializeComponent();
            GetFilters();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP)|*.BMP";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    curImage = new Bitmap(open_dialog.FileName);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Image = curImage;
                    pictureBox.Invalidate(); 
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Cant open file",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            Clear();
            curFilter = filterBox.SelectedIndex + 1;

            if ((curImage == null) || (curFilter == 0))
            {
                MessageBox.Show("Error! Nothing to be done");
                Activ();
                return;
            }

            isWorking = true;
            proccessing = new Thread(() => { Run(); });
            proccessing.IsBackground = true;
            proccessing.Start();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            isWorking = false;
            Thread stop = new Thread(() => { serviceServer.Clear(); });
            stop.IsBackground = true;
            stop.Start();
        }
    }
}
