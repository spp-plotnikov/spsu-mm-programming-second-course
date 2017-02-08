using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiltersLib
{
    public class Filters
    {
        public int Percents;
        private string[] filters = {"GrayScales", "OnlyRed", "OnlyGreen", "OnlyBlue"};
        public Filters()
        {
            Percents = 0;
        }
        public string[] GetFilters()
        {
            return filters;
        }

        public Bitmap GrayScales(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double delta = 95.0 / height;
            double temp = 0;
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    int newValue = (image.GetPixel(j, i).R + image.GetPixel(j, i).G + image.GetPixel(j, i).B) / 3;
                    image.SetPixel(j, i, Color.FromArgb(newValue, newValue, newValue));
                }
                temp += delta;
                Percents = (int)Math.Round(temp);
            }
            Percents = 95;
            return image;
        }
        public Bitmap OnlyRed(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double delta = 95.0 / height;
            double temp = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int newValue = image.GetPixel(j, i).R;
                    image.SetPixel(j, i, Color.FromArgb(newValue, 0, 0));
                }
                temp += delta;
                Percents = (int)Math.Round(temp);
            }
            Percents = 95;
            return image;
        }
        public Bitmap OnlyGreen(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double delta = 95.0 / height;
            double temp = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int newValue = image.GetPixel(j, i).G;
                    image.SetPixel(j, i, Color.FromArgb(0, newValue, 0));
                }
                temp += delta;
                Percents = (int)Math.Round(temp);
            }
            Percents = 95;
            return image;
        }
        public Bitmap OnlyBlue(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double delta = 95.0 / height;
            double temp = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int newValue = image.GetPixel(j, i).B;
                    image.SetPixel(j, i, Color.FromArgb(0, 0, newValue));
                }
                temp += delta;
                Percents = (int)Math.Round(temp);
            }
            Percents = 95;
            return image;
        }
        public Bitmap ApplyFilter(Bitmap image, string filterType)
        {
            var filters = GetFilters();
            int number = -1;
            for(int i = 0; i < filters.Length; i++)
            {
                if(filters[i].Equals(filterType))
                {
                    number = i;
                    break;
                }
            }
            if(number == 0)
            {
                return GrayScales(image);
            }
            if(number == 1)
            {
                return OnlyRed(image);
            }
            if (number == 2)
            {
                return OnlyGreen(image);
            }
            if (number == 3)
            {
                return OnlyBlue(image);
            }
            return null;
        }
    }
}
