using System;
using System.Windows.Forms;

namespace Forms
{
    public partial class ProgressBarForm : Form
    {

        public string Message
        {
            set { labelMessage.Text = value; }
        }

        public int ProgressValue
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        public ProgressBarForm()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> Canceled;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            EventHandler<EventArgs> ea = Canceled;
            if (ea != null)
                ea(this, e);
        }
    }
}
