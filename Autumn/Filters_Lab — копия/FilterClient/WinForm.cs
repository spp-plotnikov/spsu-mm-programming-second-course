﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        Thread _thread;

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
            if (SendBtn.Text == "Send")
            {
                    BeforePath.Text = "";
                    _status = 0;
                    string filter = FilterList.Text;
                    string path = BeforeFilePath.Text;
                    SendBtn.Text = "Abort";
                    _thread = new Thread(() => newImage = _client.Send(filter, path, ProgressBarUpdate));
                    _thread.Start();
                
            }
            else
            {
                _client.Abort();
                _thread.Join();
                _status = 0;
                SendBtn.Text = "Send";
            }
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            if (newImage != null)
            {
                string path = BeforeFilePath.Text.Replace(".bmp", FilterList.Text + ".bmp");
                PicAfter.Image = newImage;
                PicAfter.Image.Save(path);

                AfterPath.Text = path;

                SendBtn.Text = "Send";
                newImage = null;
            }
            progressBar.Value = _status;

            if (!(FilterList.Text.Length == 0 || BeforeFilePath.Text.Length == 0))
                SendBtn.Enabled = true;
            else
                SendBtn.Enabled = false;

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
            BeforePath.Text = "";
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