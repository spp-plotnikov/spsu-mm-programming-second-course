using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MathWinForm
{
    public partial class Form1 : Form
    {
        Form2 f2;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f2 = new Form2(int.Parse(textBox1.Text), int.Parse(textBox2.Text), combobox.SelectedIndex);
            f2.ShowDialog();
        }
    }
}