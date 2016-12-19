using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Contracts
{
    class Filters
    {
        float progress;
        float pictSize;
        byte[] result;
        Bitmap map;
        public bool Stop = false;
        int Width, Height;
        public Filters(Bitmap map)
        {
            this.map = map;
            Width = map.Width;
            Height = map.Height;
            result = new byte[map.Width * map.Height *3];
            progress = 0;
            pictSize = map.Height * map.Width;
        }

        public byte[] GreyFilter()
        {
            Stop = false;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    Color c = map.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte grey = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    progress++;
                    result[i * Height * 3 + j * 3] = grey;
                    result[i * Height * 3 + j * 3 + 1] = grey;
                    result[i * Height * 3 + j * 3 + 2] = grey;
                    if (Stop) return null;
                }
            return result;
        }

        public byte[] InvertFilter()
        {
            Stop = false;
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                {
                    Color c = map.GetPixel(i, j);
                    int red = c.R;
                    int green = c.G;
                    int blue = c.B;
                    red = 255 - red;
                    green = 255 - green;
                    blue = 255 - blue;
                    progress++;
                    result[i * map.Height * 3 + j * 3] = (byte)red;
                    result[i * map.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * map.Height * 3 + j * 3 + 2] = (byte)blue;
                    if (Stop) return null;
                }
            return result;
        }

        public byte[] SepiaFilter()
        {
            Stop = false;
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                {
                    Color c = map.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte tone = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    red = (byte)((tone > 206) ? 255 : tone + 49);
                    green = (byte)((tone < 14) ? 0 : tone - 14);
                    blue = (byte)((tone < 56) ? 0 : tone - 56);
                    result[i * map.Height * 3 + j * 3] = (byte)red;
                    result[i * map.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * map.Height * 3 + j * 3 + 2] = (byte)blue;
                    progress++;
                    if (Stop) return null;
                }           
           return result;
        }

        public float Progress()
        {
            return progress * 100.0f / pictSize;
        }
    }
}
