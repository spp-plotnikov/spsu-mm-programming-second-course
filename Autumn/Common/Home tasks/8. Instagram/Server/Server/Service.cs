using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;
using System.Drawing;
using System.ServiceModel;
using System.Configuration;

namespace Server
{
    /*
     * IService behavior
     * */

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        /*
         * Fields
         * */

        byte[] currentImage;
        private bool isWorking = false;
        private int progressbarProcent = 0;

        /*
         * Additional functions
         * */

        public UInt32[,] FromBitmapToPixel(Bitmap image)
        {
            UInt32[,] pixel = new UInt32[image.Height, image.Width];
            for (int y = 0; y < image.Height && isWorking; y++)
            {
                for (int x = 0; x < image.Width && isWorking; x++)
                {
                    pixel[y, x] = (UInt32)(image.GetPixel(x, y).ToArgb());
                }
                progressbarProcent = y * 30 / image.Height;
            }

            return pixel;
        }

        public Bitmap FromPixelToBitmap(Bitmap image, UInt32[,] pixel)
        {
            for (int y = 0; y < image.Height && isWorking; y++)
            {
                for (int x = 0; x < image.Width && isWorking; x++)
                {
                    image.SetPixel(x, y, Color.FromArgb((int)pixel[y, x]));
                }
                progressbarProcent = 50 + y * 20 / image.Height;
            }
            return image;
        }

        public byte[] FromBitmapToByte(Bitmap image)
        {
            byte[] byteImage = new byte[3 * image.Width * image.Height]; // 3 == R + G + B
            for (int i = 0; i < image.Width && isWorking; i++)
            {
                for (int j = 0; j < image.Height && isWorking; j++)
                {
                    byteImage[i * image.Height * 3 + j * 3 + 0] = (byte)image.GetPixel(i, j).R;
                    byteImage[i * image.Height * 3 + j * 3 + 1] = (byte)image.GetPixel(i, j).G;
                    byteImage[i * image.Height * 3 + j * 3 + 2] = (byte)image.GetPixel(i, j).B;
                }
                progressbarProcent = 70 + i * 30 / image.Width;
            }
            return byteImage;
        }

        private byte[] Sharpness(Bitmap image)
        {
            UInt32[,] pixel = new UInt32[image.Height, image.Width];
            if (isWorking)
            {
                pixel = FromBitmapToPixel(image);
                // 30%
                progressbarProcent = 30;
            }

            if (isWorking)
            {
                pixel = Filters.matrix_filtration(image.Width, image.Height, pixel, Filters.N1, Filters.sharpness);
                // 50%
                progressbarProcent = 50;
            }

            if (isWorking)
            {
                image = FromPixelToBitmap(image, pixel);
                //70%
                progressbarProcent = 70;
            }

            byte[] result = new byte[3 * image.Width * image.Height];
            if (isWorking)
            {
                result = FromBitmapToByte(image);
                //100%
                progressbarProcent = 100;
            }

            return result;
        }

        byte[] Blur(Bitmap image)
        {
            UInt32[,] pixel = new UInt32[image.Height, image.Width];
            if (isWorking)
            {
                pixel = FromBitmapToPixel(image);
                // 30%
                progressbarProcent = 30;
            }

            if (isWorking)
            {
                pixel = Filters.matrix_filtration(image.Width, image.Height, pixel, Filters.N2, Filters.blur);
                // 50%
                progressbarProcent = 50;
            }

            if (isWorking)
            {
                image = FromPixelToBitmap(image, pixel);
                //70%
                progressbarProcent = 70;
            }

            byte[] result = new byte[3 * image.Width * image.Height];
            if (isWorking)
            {
                result = FromBitmapToByte(image);
                //100%
                progressbarProcent = 100;
            }

            return result;
        }

        /*
         * IService funtions
         * */

        public string[] GetFilters()
        {
            string[] filters = { ConfigurationManager.AppSettings["filter1"], ConfigurationManager.AppSettings["filter2"]};
            return filters;
        }
        
        public byte[] GetImage()
        {
            return currentImage;
        }

        public int GetProgress()
        {
            return progressbarProcent;
        }

        public void Clear()
        {
            isWorking = false;
            progressbarProcent = 0;
            currentImage = null;
        }

        public void Filter(Bitmap image, int filter)
        {
            isWorking = true;

            if (filter == 1)
            {
                currentImage = Sharpness(image);
            }
            else
            {
                currentImage = Blur(image);
            }
            return;
        }
    }
}
