using SpersyApp.ServiceReference;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpersyApp
{
    class MyServiceCallback : IServiceCallback
    {
        public Bitmap Pic;
        public int Percents;

        public void GetImage(byte[] image)
        {
            Pic = (Bitmap)Image.FromStream(new MemoryStream(image));
        }

        public void GetProgress(int progress)
        {
            Percents = progress;
        }
    }
}
