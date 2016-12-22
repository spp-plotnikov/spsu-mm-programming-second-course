using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterClient
{
    public partial class WinForm : Form
    {
        private BmpClient _client;
        Image newImage = null;
        int _status = 0;

        public WinForm()
        {
            InitializeComponent();
            Application.Idle += ApplicationIdle;
        }

        private void SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "bmp files (*.bmp)|*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _status = 0;
                BeforeFilePath.Text = openFileDialog.FileName;
                PicBefore.ImageLocation = openFileDialog.FileName;
            }
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            if (FilterList.Text.Length > 0 && BeforeFilePath.Text.Length > 0)
            {
                _status = 0;
                string filter = FilterList.Text;
                string path = BeforeFilePath.Text;

                Thread thread = new Thread(() => newImage = _client.Send(filter, path, ProgressBarUpdate));
                thread.Start();
            }
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            if (newImage != null)
            {
                PicAfter.Image = newImage;
                newImage = null;
            }
            progressBar.Value = _status;
        }

        private void ProgressBarUpdate(int status)
        {
            _status = status;
        }
        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            FilterList.Items.Clear();
            _status = 0;
            ServerAdress.Text = "";
            _client = new BmpClient(ServerText.Text);
            try
            {
                string[] tmp = _client.Init();

                ServerAdress.Text = ServerText.Text;
                foreach (string i in tmp)
                    FilterList.Items.Add(i);
            }
            catch
            {
                ServerAdress.Text = "ConnectError";
            }
        }


    }
}
