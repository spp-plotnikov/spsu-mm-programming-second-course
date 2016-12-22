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
    public partial class MainForm : Form
    {
        private string path;
        private Bitmap curImage;
        private Filtering.ServiceClient host;
        ManualResetEvent progressFinished = new ManualResetEvent(true); // Signals that checkingProgressWorker finished
        bool cancelled = false; 
        bool finished = true; // Becomes true, when sendSetFilterNReceiveWorker displayed result image
        ManualResetEvent sendSetFilterNReceiveWorkerIsFree = new ManualResetEvent(true);

        public MainForm()
        { 
            InitializeComponent();
            this.host = new Filtering.ServiceClient();
        }

        private void openImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();            
            openFileDialog.Filter = "Images (*.bmp; *.jpg)|*.jpg; *.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            try
            {
                BeforeImageBox.Image = new Bitmap(openFileDialog.FileName);
                curImage = new Bitmap(BeforeImageBox.Image);
                path = openFileDialog.FileName;
                fileName.Text = path;
            }
            catch { }
        }
        
        private void sendImageButton_Click(object sender, EventArgs e)
        {            
            sendImageButton.Enabled = false;
            if (!sendSetFilterNReceiveWorker.IsBusy)
                sendSetFilterNReceiveWorker.RunWorkerAsync();
        }
        
        private void updateFilterListButton_Click(object sender, EventArgs e)
        {            
            BindingSource filters = new BindingSource();
            filters.DataSource = host.Filters();
            filterNameComboBox.DataSource = filters;            
        }

        private void SendNGetImage(object sender, DoWorkEventArgs e)
        {
            sendSetFilterNReceiveWorkerIsFree.Reset();
            bool error = false;
            this.Invoke((MethodInvoker)delegate ()
            {
                if (curImage == null) { MessageBox.Show("No image"); error = true; sendImageButton.Enabled = true; return; }
                if (!(filterNameComboBox.Items.Count > 0)) { MessageBox.Show("Choose filter!"); error = true; sendImageButton.Enabled = true; return; }
            });
            if (error) { sendSetFilterNReceiveWorkerIsFree.Set(); return; }
            cancelled = false;
            ConnectionMethods.Cancelled = false;            
            finished = false;
            byte[] toSend = ConnectionMethods.ImageToByteArray(curImage);
            this.Invoke((MethodInvoker)delegate ()
            {
                host.SetFilter(filterNameComboBox.SelectedValue.ToString());
            });
            if (!checkingProgressWorker.IsBusy) { checkingProgressWorker.RunWorkerAsync(); }
            if (cancelled) { sendSetFilterNReceiveWorkerIsFree.Set(); return; }
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            if (cancelled) { sendSetFilterNReceiveWorkerIsFree.Set(); return; }
            host.DoFilter();
            if (cancelled) { sendSetFilterNReceiveWorkerIsFree.Set(); return; }
            byte[] result = ConnectionMethods.receiveByteArrayUsingChunks(host);
            if (!cancelled)
            {                
                AfterImageBox.Image = ConnectionMethods.byteArrayToBitmap(result);
            }
            else
            {
                sendSetFilterNReceiveWorkerIsFree.Set();
                return;
            }
            finished = true;
            this.Invoke((MethodInvoker)delegate ()
            {                
                progressFinished.WaitOne();
                progressBar.Value = 100;
                statusLabel.Text = "done";
                progressBar.Value = 0;
                sendImageButton.Enabled = true;
                sendSetFilterNReceiveWorkerIsFree.Set();
            });
        }

        private void UpdatingProgressBar(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            progressFinished.Reset();
            this.Invoke((MethodInvoker)delegate ()
            {
                statusLabel.Text = "sending";
            });
            progressBar.Maximum = 100;
            progressBar.Minimum = 0;
            while (!cancelled)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    progress = host.GetProgress();
                    if (Math.Abs(progressBar.Value - progress) < 90)
                    {
                        if (progress < 99 && progress > 0)
                        {
                            progressBar.Value = progress;
                            statusLabel.Text = "filtering";
                        }
                        if (progress >= 98)
                        {
                            progressBar.Value = progress;
                            statusLabel.Text = "recieving";
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
            if (!cancellationWorker.IsBusy) { cancellationWorker.RunWorkerAsync(); }
        }
        
        private void Cancellation(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cancelButton.Enabled = false;
                statusLabel.Text = "cancellation";
                cancelled = true;
                host.Cancel();
                ConnectionMethods.Cancelled = true;
                progressBar.Value = 0;
                sendSetFilterNReceiveWorkerIsFree.WaitOne();
                statusLabel.Text = "cancelled";
                sendImageButton.Enabled = true;
                cancelButton.Enabled = true;
            });
        }
    }
}
