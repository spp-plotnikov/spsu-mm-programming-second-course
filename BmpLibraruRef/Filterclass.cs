using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibraruRef
{
    public class Filterclass
    {

        public static byte[] grey(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {
            int R = 0, G = 0, B = 0, Y = 0;

            for (int i = 0; i < biHeight; i++)
                for (int j = 0; j < biWidth; j++)
                {
                    R = bitecolor[i, j].rgbtRed;
                    G = bitecolor[i, j].rgbtGreen;
                    B = bitecolor[i, j].rgbtBlue;
                    Y = (R + G + B) / 3;
                    bitecolor[i, j].rgbtRed = (byte)Y;
                    bitecolor[i, j].rgbtGreen = (byte)Y;
                    bitecolor[i, j].rgbtBlue = (byte)Y;
                }

            return data;
        }

        public static byte[] createbmp(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor)
        {
            int k = 54;
            for (int i = 0; i < biHeight; i++)
            {
                for (int j = 0; j < biWidth; j++)
                {
                    data[k] = bitecolor[i, j].rgbtRed;
                    k++;
                    data[k] = bitecolor[i, j].rgbtGreen;
                    k++;
                    data[k] = bitecolor[i, j].rgbtBlue;
                    k++;
                    if (biBitCount == 32) { k++; }
                }
                if (biBitCount == 24) k += biWidth % 4;
            }
            return data;

        }

        public static void Filter(string dir)
        {
            byte[] data = System.IO.File.ReadAllBytes(@dir);

            int biWidth = BitConverter.ToInt32(data, 18);        // Ширина изображения в пикселях
            int biHeight = BitConverter.ToInt32(data, 22);        // Высота изображения в пикселях
            int biBitCount = BitConverter.ToInt16(data, 28);      // Бит/пиксел: 32 или 24 

            Color[,] bitecolor = new Color[biHeight, biWidth];
            Color[,] bitecolorcopy = new Color[biHeight, biWidth];


            int k = 54;
            for (int i = 0; i < biHeight; i++)
            {

                for (int j = 0; j < biWidth; j++)
                {
                    bitecolor[i, j].rgbtRed = data[k];
                    bitecolorcopy[i, j].rgbtRed = data[k];
                    k++;
                    bitecolor[i, j].rgbtGreen = data[k];
                    bitecolorcopy[i, j].rgbtGreen = data[k];
                    k++;
                    bitecolor[i, j].rgbtBlue = data[k];
                    bitecolorcopy[i, j].rgbtBlue = data[k];
                    k++;
                    if (biBitCount == 32) { k++; }
                }
                if (biBitCount == 24) k += biWidth % 4;
            }

            data = grey(data, biWidth, biHeight, biBitCount, bitecolor, bitecolorcopy);
            createbmp(data, biWidth, biHeight, biBitCount, bitecolor); 
            string path = dir + ".result.bmp";
            Console.WriteLine(path);
            System.IO.File.WriteAllBytes(path, data);
            

        }
    }
}
    