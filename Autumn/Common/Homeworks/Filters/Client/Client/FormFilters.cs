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
        private Thread _threadFilter, _threadLoad, _threadProgress;
        delegate void Proc();
        private bool _isProcess = true;
        private Bitmap _image;
        private IService _filter;

        public FormFilters(IService server)
        {
            _filter = server;
            InitializeComponent();
            GetFilters();
        }

        private void GetFilters()
        {
            foreach (string fi in _filter.GetFilters())
            {
                FiltersList.Items.Add(fi);
            }
        }

        private void ResetThreads()
        {
            ProgressBar.Value = 0;
            _filter.CancelProgress();
            if ((_filter.CheckIsAlive()) && (_threadFilter != null) && (_threadProgress != null))
            {
                _filter.ChangeIsAlive(false);
                _isProcess = false;
            }
        }

        private void LoadImage(string addressRead)
        {
            var fileLength = new FileInfo(addressRead).Length; 
            if (fileLength > 7 * 1024 * 1024)
            {
                MessageBox.Show("Ошибка! Файл должен быть меньше 7 MB");
                return;
            }
            _image = new Bitmap(addressRead);
            PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBox.Image = _image;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            ResetThreads();
            FileDialog.Filter = "Изображения |*.bmp";
            FileDialog.FileName = "";
            FileDialog.ShowDialog();
            string addressRead = FileDialog.FileName;
            _threadLoad = new Thread(delegate() { LoadImage(addressRead); });
            _threadLoad.IsBackground = true;
            _threadLoad.Start();
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            ResetThreads();
            _image = new Bitmap(PictureBox.Image);
            int index = FiltersList.SelectedIndex + 1;

            if ((index == 0) || (_image == null))
            {
                MessageBox.Show("Ошибка! Не все поля заполнены!");
                return;
            }

            int max = ProgressBar.Maximum;
            int width = _image.Width;

            _filter.SetIndex(index);
            _threadFilter = new Thread(delegate() { _filter.Filter(_image); });
            _threadFilter.IsBackground = true;
            _threadFilter.Start();

            _threadProgress = new Thread(delegate() { Process(max, width); });
            _threadProgress.IsBackground = true;
            _threadProgress.Start();

            _isProcess = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ResetThreads();
        }

        private void Process(int max, int width)
        {
            int newValue = 0;
            _filter.CancelProgress();
            try
            {
                do
                {
                    newValue = max * _filter.GetProgress() / width;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { ProgressBar.Value = newValue; }));
                    }
                    else
                    {
                        ProgressBar.Value = newValue;
                    }
                }
                while (_filter.CheckIsAlive());

                if (_isProcess)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { ProgressBar.Value = max; }));
                        this.Invoke(new Proc(delegate() { PictureBox.Image = ToBitMap(_filter.GetImage()); }));
                    }
                    else
                    {
                        ProgressBar.Value = max;
                        PictureBox.Image = ToBitMap(_filter.GetImage());
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        private Bitmap ToBitMap(byte[] array)
        {
            Bitmap res = new Bitmap(_image.Width, _image.Height);
            for (int i = 0; i < _image.Width; i++)
            {
                for (int j = 0; j < _image.Height; j++)
                {
                    res.SetPixel(i, j, Color.FromArgb(array[i * _image.Height * 3 + j * 3],
                                                      array[i * _image.Height * 3 + j * 3 + 1],
                                                      array[i * _image.Height * 3 + j * 3 + 2]));
                }
            }
            return res;
        }
    }
}
