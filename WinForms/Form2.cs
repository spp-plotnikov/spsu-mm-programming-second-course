using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MathWinForm
{
    public partial class Form2 : Form
    {

        int begin, end, curve; 
       
        public Form2(int begin, int end, int curve)
        {
            InitializeComponent();
            this.curve = curve;//гипербола/бэт/синус
            this.begin = begin;
            this.end = end;

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gr = Graphics.FromImage(bmp);
             // сетка
                for (int xy = 0; xy < 600; xy += 10)
                {
                    gr.DrawLine((new Pen(Color.Gray)), 0, xy, 600, xy);
                    gr.DrawLine((new Pen(Color.Gray)), xy, 0, xy, 600);
                }

                gr.DrawLine((new Pen(Color.Black)), 0, 300, 600, 300);
                gr.DrawLine((new Pen(Color.Black)), 300, 0, 300, 600);
                //
            double step = (Math.Abs(end) - begin);
            step = step / 10.0;
            Formula graf = new Formula();
            double y = 0;
            if (curve == -1) { y = graf.Hyperbola(begin, end, begin); }
            if (curve == 1) { y = graf.Cubic(begin, end, begin); }
            if (curve == 2) { y = graf.Oval(begin, end, begin); }

            Point last = new Point((int)((begin + 300)), (int)((y + 300)));
            for (double x = begin; x < end; x += step)
            {
                if (curve == -1) y = graf.Hyperbola(begin, end, x);
                if (curve == 1) { y = graf.Cubic(begin, end, x); }
                if (curve == 2) { y = graf.Oval(begin, end, x); }
                Point now = new Point((int)((x + 300)), (int)((y + 300)));
                gr.DrawLine((new Pen(Color.DarkGreen, 2)), now, last);
                last = new Point((int)((x + 300)), (int)((y + 300))); ;
            }

           // this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.Image = bmp;
            this.pictureBox1.Size = bmp.Size;

        }
        int lx = 0, ly = 0;
        private void vScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            pictureBox1.Size = new Size(vScrollBar1.Value*5, vScrollBar1.Value *5);
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) //Метод-обработчик отпускания мыши
        {
            int dx = e.X - lx;
            int dy = e.Y - ly;
            pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X + dx, pictureBox1.Location.Y + dy);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) //Метод-обработчик задержки мыши
        {
            lx = e.X;
            ly = e.Y;
        }


    }
}





