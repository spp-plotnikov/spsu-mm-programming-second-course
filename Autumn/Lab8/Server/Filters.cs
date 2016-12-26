using System;
using System.Collections.Generic;
using System.Drawing;

namespace Server
{
    public class Filters
    {
        public bool CancelProcess;
        public int Progress;

        public Filters()
        {
            CancelProcess = false;
            Progress = 0;
        }

        public Bitmap Negative(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    image.SetPixel(i, j, Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B));
                    if (CancelProcess)
                    {
                        Progress = 75;
                        return null;
                    }
                }
            }
            Progress = 75;
            return image;
        }

        public Bitmap Gray(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    int gr = (color.R + color.G + color.B) / 3;
                    image.SetPixel(i, j, Color.FromArgb(gr, gr, gr));
                    if (CancelProcess)
                    {
                        Progress = 75;
                        return null;
                    }
                }
            }
            Progress = 75;
            return image;
        }

        public Bitmap Contrast(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            int contrastLevel = 2;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    int red = ((color.R - 128) * contrastLevel + 128);
                    red = red > 0 ? (red < 255 ? red : 255) : 0;

                    int green = ((color.G - 128) * contrastLevel + 128);
                    green = green > 0 ? (green < 255 ? green : 255) : 0;

                    int blue = ((color.B - 128) * contrastLevel + 128);
                    blue = blue > 0 ? (blue < 255 ? blue : 255) : 0;

                    image.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    if (CancelProcess)
                    {
                        Progress = 75;
                        return null;
                    }
                }
            }
            Progress = 75;
            return image;
        }

        public Bitmap MakeChanging(Bitmap image, string filter)
        {
            List<string> filters = new List<string> { "Negative", "Gray", "Contrast" };
            int filterIndex = 10;
            for (int i = 0; i < 5; i++)
            {
                if (String.Equals(filter, filters[i]))
                {
                    filterIndex = i;
                    break;
                }
            }
            Progress = 35;
            switch (filterIndex)
            {
                case 0:
                    return Negative(image);
                case 1:
                    return Gray(image);
                case 2:
                    return Contrast(image);
            }
            return null;
        }
    }
}
