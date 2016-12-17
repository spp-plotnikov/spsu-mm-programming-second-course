using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;
using System.Drawing;

namespace Server
{
    public class Service : IService
    {
        public bool IsAlive = false;
        public int Progress = 0;
        public Bitmap Image;
        byte[] result;
        int index;

        public void GetIndex(int i)
        {
            index = i;
        }

        public string[] GetFilters()
        {
            string[] filters = {
            "Оттенки серого",
            "Инвертирование",
            "Сепия",
            };
            return filters;
        }

        public void SetProgress()
        {
            Progress = 0;
        }

        public bool CheckIsAlive()
        {
            return IsAlive;
        }

        public void ChangeIsAlive(bool x)
        {
            IsAlive = x;
        }

        public int GetProgress()
        {
            return Progress;
        }

        public byte[] GetImage()
        {
            return result;
        }

        private void ProgressIncr()
        {
            Progress++;
        }

        public void Filter(Bitmap image)
        {
            Image = image;
            IsAlive = true;
            Bitmap newImage = Image;

            result = new byte[image.Width * image.Height * 3];

            switch (index)
            {
                case 1:
                    {
                        GreyFilter();
                        break;
                    }
                case 2:
                    {
                        InvertFilter();
                        break;
                    }

                case 3:
                    {
                        SepiaFilter();
                        break;
                    }
            }
            IsAlive = false;
        }

        public void GreyFilter()
        {
            IsAlive = true;
            for (int i = 0; i < Image.Width; i++)
            {
                for (int j = 0; j < Image.Height; j++)
                {
                    Color c = Image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte grey = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    result[i * Image.Height * 3 + j * 3] = grey;
                    result[i * Image.Height * 3 + j * 3 + 1] = grey;
                    result[i * Image.Height * 3 + j * 3 + 2] = grey;
                    if (!IsAlive) return;
                }
                ProgressIncr();
            }
        }

        public void InvertFilter()
        {
            IsAlive = true;
            for (int i = 0; i < Image.Width; i++)
            {
                for (int j = 0; j < Image.Height; j++)
                {
                    Color c = Image.GetPixel(i, j);
                    int red = c.R;
                    int green = c.G;
                    int blue = c.B;
                    red = 255 - red;
                    green = 255 - green;
                    blue = 255 - blue;
                    result[i * Image.Height * 3 + j * 3] = (byte)red;
                    result[i * Image.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * Image.Height * 3 + j * 3 + 2] = (byte)blue;
                    if (!IsAlive) return;
                }
                ProgressIncr();
            }
        }

        public void SepiaFilter()
        {
            IsAlive = true;
            for (int i = 0; i < Image.Width; i++)
            {
                for (int j = 0; j < Image.Height; j++)
                {
                    Color c = Image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte tone = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    red = (byte)((tone > 206) ? 255 : tone + 49);
                    green = (byte)((tone < 14) ? 0 : tone - 14);
                    blue = (byte)((tone < 56) ? 0 : tone - 56);
                    result[i * Image.Height * 3 + j * 3] = (byte)red;
                    result[i * Image.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * Image.Height * 3 + j * 3 + 2] = (byte)blue;
                    if (!IsAlive) return;
                }
                ProgressIncr();
            }
        }
    }
}
