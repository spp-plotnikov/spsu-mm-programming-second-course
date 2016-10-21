using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibraruRef
{
    public class Filterclass
    {

        public static byte[] DoGrey(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] byteColor, Color[,] byteColorCopy)
        {
            int R = 0, G = 0, B = 0, Y = 0;

            for (int i = 0; i < biHeight; i++)
                for (int j = 0; j < biWidth; j++)
                {
                    R = byteColor[i, j].RgbtRed;
                    G = byteColor[i, j].RgbtGreen;
                    B = byteColor[i, j].RgbtBlue;
                    Y = (R + G + B) / 3;
                    byteColor[i, j].RgbtRed = (byte)Y;
                    byteColor[i, j].RgbtGreen = (byte)Y;
                    byteColor[i, j].RgbtBlue = (byte)Y;
                }

            return data;
        }

        public static byte[] CreateBmp(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] byteColor)
        {
            int k = 54;
            for (int i = 0; i < biHeight; i++)
            {
                for (int j = 0; j < biWidth; j++)
                {
                    data[k] = byteColor[i, j].RgbtRed;
                    k++;
                    data[k] = byteColor[i, j].RgbtGreen;
                    k++;
                    data[k] = byteColor[i, j].RgbtBlue;
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

            Color[,] byteColor = new Color[biHeight, biWidth];
            Color[,] byteColorCopy = new Color[biHeight, biWidth];


            int k = 54;
            for (int i = 0; i < biHeight; i++)
            {

                for (int j = 0; j < biWidth; j++)
                {
                    byteColor[i, j].RgbtRed = data[k];
                    byteColorCopy[i, j].RgbtRed = data[k];
                    k++;
                    byteColor[i, j].RgbtGreen = data[k];
                    byteColorCopy[i, j].RgbtGreen = data[k];
                    k++;
                    byteColor[i, j].RgbtBlue = data[k];
                    byteColorCopy[i, j].RgbtBlue = data[k];
                    k++;
                    if (biBitCount == 32) { k++; }
                }
                if (biBitCount == 24) k += biWidth % 4;
            }

            data = DoGrey(data, biWidth, biHeight, biBitCount, byteColor, byteColorCopy);
            CreateBmp(data, biWidth, biHeight, biBitCount, byteColor); 
            string path = dir + ".result.bmp";
            Console.WriteLine(path);
            System.IO.File.WriteAllBytes(path, data);
            

        }
    }
}
    