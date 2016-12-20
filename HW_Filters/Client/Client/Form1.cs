using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Contracts;
using System.IO;

namespace Client
{
    public partial class Form1 : Form
    {
        private Bitmap _image;
        private IService1 _myService;
        private Thread _filter;
        private List<string> _listOfFilters;


        public Form1(IService1 service)
        {
            _listOfFilters = new List<string>();
            _myService = service;
            InitializeComponent();
            _addFilters();
        }

        private void _addFilters()
        {
            foreach (string filter in _myService.GetListOfFilters())
            {
                this.ChoiceButton.Items.Add(filter);
                _listOfFilters.Add(filter);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        

        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }


        private void name_Click(object sender, EventArgs e)
        {

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

            this.progressBar1.Value = 0;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image File |*.bmp";

            if (file.ShowDialog() == DialogResult.OK)
            {
                _image = new Bitmap(file.FileName);
                this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox.Image = _image;
            }
        }

        private void GoButton_Click(object sender, EventArgs e)
        {

            //string nameOfFilter = _myService.GetListOfFilters()[this.ChoiceButton.SelectedIndex];
          //  this.textBox1.Text = this.ChoiceButton.SelectedIndex.ToString();
            //this.textBox1.Show();
            int idx = this.ChoiceButton.SelectedIndex;
            _filter = new Thread(() => { _image = _myService.ApplyFilter(_image, _listOfFilters[idx]);  });
            _filter.IsBackground = true;
            _filter.Start();

            Thread progress = new Thread(() => { Progres(); });
            progress.IsBackground = true;
            progress.Start();

        }

        private void Progres()
        {
            int progress = 0;
            this.progressBar1.Maximum = 100;
            while (progress != 100)
            {
                progress = _myService.GetProgress();
                this.progressBar1.Value = progress;
            }
            while (_filter.IsAlive) { }
            this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox.Image = _image;
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            Thread stop = new Thread(() => { _myService.Stop(); });
            stop.IsBackground = true;
            stop.Start();
        }

        private void ChoiceButton_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
