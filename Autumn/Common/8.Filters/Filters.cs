using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

class Filters
{
    public Bitmap Filter1(Bitmap src, ref double progress)
    {
        progress = 0;

        double step = 1.0 / src.Width;

        Bitmap filtered = new Bitmap(src);
        for (int i = 0; i < filtered.Width; ++i)
        {
            for (int j = 0; j < filtered.Height; ++j)
            {
                Color pixelColor = filtered.GetPixel(i, j);
                Color newColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                filtered.SetPixel(i, j, newColor);
            }
            progress += step;
        }
        return filtered;
    }

    public Bitmap Filter2(Bitmap src, ref double progress)
    {
        progress = 0;

        double rate = 1.0 / src.Width;

        Bitmap filtered = new Bitmap(src);
        for (int i = 0; i < filtered.Width; i++)
        {
            for (int j = 0; j < filtered.Height; j++)
            {
                Color pixelColor = filtered.GetPixel(i, j);
                Color newColor = Color.FromArgb(0, pixelColor.G, 0);
                filtered.SetPixel(i, j, newColor);
            }
            progress += rate;
        }
        return filtered;
    }

    public Bitmap Filter3(Bitmap src, ref double progress)
    {
        progress = 0;

        const int compressDeep = 20;
        const long powCompressDeep = compressDeep * compressDeep;

        double step = 1 / ((double)src.Width) * compressDeep;
       
        Bitmap filtered = new Bitmap(src.Width - src.Width % compressDeep, src.Height - src.Height % compressDeep);
        for (int x = 0; x < src.Width - compressDeep; x += compressDeep)
        {
            for (int y = 0; y < src.Height - compressDeep; y += compressDeep)
            {
                long red = 0, green = 0, blue = 0;
                for (int i = 0; i < compressDeep; ++i)
                {
                    for (int j = 0; j < compressDeep; ++j)
                    {
                        red += src.GetPixel(x + i, y + j).R;
                        green += src.GetPixel(x + i, y + j).G;
                        blue += src.GetPixel(x + i, y + j).B;
                    }
                }
                Color newColor = Color.FromArgb((int)(red / powCompressDeep), (int)(green / powCompressDeep), (int)(blue / powCompressDeep));
                for (int i = 0; i < compressDeep; i++)
                {
                    for (int j = 0; j < compressDeep; j++)
                    {
                        filtered.SetPixel(x + i, y + j, newColor);
                    }
                }
            }
            progress += step;
        }

        return filtered;
    }
}

