using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<KeyValuePair<int, long>> result = new List<KeyValuePair<int, long>>();
            Bitmap image = new Bitmap("test.bmp");
            int height = image.Height;
            int width = image.Width;
            for(int i = 1; i < 15; i++)
            {
                var client = new Test();
                var newImage = ResizeImage(image, i * width, i * height);
                long time = client.Testing((byte[])(new ImageConverter().ConvertTo(newImage, typeof(byte[]))));
                result.Add(new KeyValuePair<int, long>(i, time));
                Console.WriteLine(i);
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("ImageTest.txt"))
            {
                foreach (var pair in result)
                {
                    file.WriteLine(pair.Key + " " + pair.Value / pair.Key);
                }
            }
            Console.WriteLine("Success!");
            Console.ReadLine();
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
