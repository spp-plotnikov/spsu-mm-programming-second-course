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

namespace Client
{
    public partial class FormFilters : Form
    {
        int newValue = 0;
        Thread thread, thread1;
        string AddressRead;
        delegate void Proc();
        bool IsProcess = true;
        Bitmap Image;
        IService filter;
        bool check = false;
       // FilterClass filter = new FilterClass();
        public FormFilters(IService server)
        {
            filter = server;
            InitializeComponent();
            GetFilters();
        }

        private void GetFilters()
        {
            foreach (string fi in filter.GetFilters())
            {
                FiltersList.Items.Add(fi);
            }
        }
        private void ResetThreads()
        {
            progressBar1.Value = newValue = 0;
            filter.SetProgress();
            if ((filter.CheckIsAlive()) && (thread != null) && (thread1 != null))
            {
                filter.ChangeIsAlive(false);
                IsProcess = false;
            }
        }

        private void LoadImage(string AddressRead)
        {
            try
            {
                Image = new Bitmap(AddressRead);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при считывании");
                return;
            }

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = Image;
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            ResetThreads();

            FileDialog.Filter = "Изображения |*.bmp";
            FileDialog.FileName = "";
            FileDialog.ShowDialog();

            AddressRead = FileDialog.FileName;

            thread = new Thread(delegate() { LoadImage(AddressRead); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void Filter_Button_Click(object sender, EventArgs e)
        {
            ResetThreads();
            Image = new Bitmap(pictureBox.Image);

            int index = FiltersList.SelectedIndex + 1;
            if ((index == 0) || (Image == null))
            {
                MessageBox.Show("ERROR! PLease, try again!!!");
                return;
            }

            int Max = progressBar1.Maximum;
            int Width = Image.Width;

            filter.GetIndex(index);
            thread = new Thread(delegate() { filter.Filter(Image); });
            thread.IsBackground = true;
            thread.Start();

            thread1 = new Thread(delegate() { Process(Max, Width); });
            thread1.IsBackground = true;
            thread1.Start();

            IsProcess = true;
        }

        private Bitmap ToBitMap(byte[] array)
        {
            Bitmap res = new Bitmap(Image.Width, Image.Height);
            for (int i = 0; i < Image.Width; i++)
                for (int j = 0; j < Image.Height; j++)
                {
                    res.SetPixel(i, j, Color.FromArgb(array[i * Image.Height * 3 + j * 3],
                                                      array[i * Image.Height * 3 + j * 3 + 1],
                                                      array[i * Image.Height * 3 + j * 3 + 2]));
                }
            return res;
        }
        private void Process(int Max, int Width)
        {
            newValue = 0;
            filter.SetProgress();
            try
            {
                do
                {
                    newValue = Max * filter.GetProgress() / Width;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { progressBar1.Value = newValue; }));
                    }
                    else
                        progressBar1.Value = newValue;
                }
                while (filter.CheckIsAlive());

                if (IsProcess)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { progressBar1.Value = Max; }));
                        this.Invoke(new Proc(delegate() { pictureBox.Image = ToBitMap(filter.GetImage()); }));
                    }
                    else
                    {
                        progressBar1.Value = Max;
                        pictureBox.Image = ToBitMap(filter.GetImage());
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ResetThreads();
        }
    }
}
