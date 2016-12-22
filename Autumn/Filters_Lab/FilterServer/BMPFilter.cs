using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FilterServer
{
    static class BMPFilter
    {
        public delegate void update(int status);
        public static Bitmap ApplyFilter(Bitmap src, string filter, Stream io)
        {
            Bitmap dst = (Bitmap)src.Clone();
            
            DateTime startTime = DateTime.Now;
            int status = 0;
            long size = src.Width * src.Height;
            TimeSpan interval = new TimeSpan(10000000);
            for (int x = 0; x < dst.Width; x++)
                for (int y = 0; y < dst.Height; y++)
                {
                    Color color = src.GetPixel(x, y);
                    int newColor = (color.R + color.G + color.B)/3;
                    dst.SetPixel(x, y, Color.FromArgb(newColor,newColor,newColor));
                    //dst.SetPixel(x, y, dst.GetPixel(x,y));
                    if (DateTime.Now - startTime >= interval)
                    {
                        startTime = DateTime.Now;
                        double count = (x * src.Height + y + 1) / (double)size;
                        status = (int)(count * 100);
                        io.WriteByte(Convert.ToByte(status));
                    }
                }

            if (status != 100)
                io.WriteByte(Convert.ToByte(100));

            return dst;

        }

    }
}
