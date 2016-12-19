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
        private Thread _threadFilter, _threadLoad, _threadProcess;
        delegate void Proc();
        private Bitmap _image;
        private Bitmap _prevIm;
        private IService _filter;
        int _index;
        byte[] res = null;
        bool _isProcess = false;

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
            PictureBox.Image = (Bitmap)_image.Clone();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
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
            _image = new Bitmap(PictureBox.Image);
            ProgressBar.Value = 0;
            _filter.CancelProgress();
            FilterButton.Enabled = false;
            _index = FiltersList.SelectedIndex + 1;

            if ((_index == 0) || (_image == null))
            {
                MessageBox.Show("Ошибка! Не все поля заполнены!");
                FilterButton.Enabled = true;
                return;
            }

            _isProcess = true;
            _threadProcess = new Thread(() => { Run(); });
            _threadProcess.IsBackground = true;
            _threadProcess.Start();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Thread stop = new Thread(() => { _filter.ChangeIsAlive(false); });
            stop.IsBackground = true;
            stop.Start();
            _isProcess = false;
        }

        private void Run()
        {
            Bitmap old = new Bitmap(_image);
            _threadFilter = new Thread(() => { res = (_filter.Filter(_image, _index)); });
            _threadFilter.IsBackground = true;
            _threadFilter.Start();
            int progress = 0;
            this.ProgressBar.Maximum = 100;
            while (progress != 100 && _isProcess)
            {
                Thread.Sleep(10);
                progress = _filter.GetProgress();
                if ((this.InvokeRequired))
                {
                    this.Invoke(new Proc(delegate() { ProgressBar.Value = progress; }));
                }
                else
                {
                    ProgressBar.Value = progress;
                }
            }
            if (_filter.CheckIsAlive())
            {
                _image = new Bitmap(ToBitMap(res));
            }
            else
            {
                _image = new Bitmap(old);
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Proc(delegate() { PictureBox.Image = _image;}));
                this.Invoke(new Proc(delegate() { FilterButton.Enabled = true;}));
            }
            else
            {
                PictureBox.Image = _image;
                FilterButton.Enabled = true;
            }
            _isProcess = false;
        }

        private Bitmap ToBitMap(byte[] array)
        {
            Bitmap res = new Bitmap(_image.Width, _image.Height);
            if (_isProcess)
            {
                for (int i = 0; i < _image.Width; i++)
                {
                    for (int j = 0; j < _image.Height; j++)
                    {
                        res.SetPixel(i, j, Color.FromArgb(array[i * _image.Height * 3 + j * 3],
                                                          array[i * _image.Height * 3 + j * 3 + 1],
                                                          array[i * _image.Height * 3 + j * 3 + 2]));
                    }
                }
            }
            return res;
        }
    }
}
