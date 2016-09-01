using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace filtersharp2
{
    class Program
    {

        public static void DoMedian(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {
            byte[] R = new byte[9];
            byte[] G = new byte[9];
            byte[] B = new byte[9];

            for (int a = 1; a <  biHeight - 1; a++)
                for (int b = 1; b <  biWidth - 1; b++)
                {
                    int k = 0;
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            R[k] = bitecolor[i + a, j + b].rgbtRed;
                            G[k] = bitecolor[i + a, j + b].rgbtGreen;
                            B[k] = bitecolor[i + a, j + b].rgbtBlue;
                            k++;
                        }
                    Array.Sort(R);
                    Array.Sort(G);
                    Array.Sort(B);
                    bitecolorcopy[a, b].rgbtBlue = B[4];
                    bitecolorcopy[a, b].rgbtRed = R[4];
                    bitecolorcopy[a, b].rgbtGreen = G[4];
                }
        }

        public static void DoGauss(int r, byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {

            double[,] gaussArray = new double[5, 5];
            double div = 0;
            int pixelPosX, pixelPosY;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    gaussArray[i, j] = (double)(1 / (3.14 * 2 * r * r)) * Math.Exp((Math.Abs((5 / 2) - i) * Math.Abs((5 / 2) - i)) * (-1.0) / (2 * r * r));
                    div += gaussArray[i, j];
                }

            for (int y = 2; y <  biHeight - 2; y++)
                for (int x = 2; x <  biWidth - 2; x++)
                {
                    double rSum = 0, gSum = 0, bSum = 0, kSum = 0;
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 5; j++)
                        {
                            pixelPosX = x + j - 2;
                            pixelPosY = y + i - 2;

                            byte red = bitecolorcopy[pixelPosY, pixelPosX].rgbtRed;
                            byte green = bitecolorcopy[pixelPosY, pixelPosX].rgbtGreen;
                            byte blue = bitecolorcopy[pixelPosY, pixelPosX].rgbtBlue;

                            rSum += red * gaussArray[i, j];
                            gSum += green * gaussArray[i, j];
                            bSum += blue * gaussArray[i, j];

                            kSum += gaussArray[i, j];
                        }

                    if (kSum <= 0) kSum = 1;

                    rSum /= kSum;
                    if (rSum < 0) rSum = 0;
                    if (rSum > 255) rSum = 255;

                    gSum /= kSum;
                    if (gSum < 0) gSum = 0;
                    if (gSum > 255) gSum = 255;

                    bSum /= kSum;
                    if (bSum < 0) bSum = 0;
                    if (bSum > 255) bSum = 255;

                    bitecolorcopy[y, x].rgbtRed = (byte)rSum;
                    bitecolorcopy[y, x].rgbtGreen = (byte)gSum;
                    bitecolorcopy[y, x].rgbtBlue = (byte)bSum;
                }

        }



        public static void DoSobelx(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {
            int[,] g = new int[,] {  { -1, 0, 1 }, 
                                     { -2, 0, 2 }, 
                                     { -1, 0, 1 }};

            int[,] gy = new int[,] { { 1, 2, 1 }, 
                                     { 0, 0, 0 }, 
                                     {-1,-2,-1}};

            int newry = 0, newgy = 0, newby = 0;
            int rcy, gcy, bcy;
            int newr = 0, newg = 0, newb = 0;
            int rc, gc, bc;

            for (int i = 1; i <  biHeight - 1; i++)
                for (int j = 1; j <  biWidth - 1; j++)
                {
                    newry = 0; newgy = 0; newby = 0;
                    rcy = 0; gcy = 0; bcy = 0;
                    newr = 0; newg = 0; newb = 0;

                    for (int wi = -1; wi < 2; wi++)
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rcy = bitecolor[i + hw, j + wi].rgbtRed;
                            bcy = bitecolor[i + hw, j + wi].rgbtBlue;
                            gcy = bitecolor[i + hw, j + wi].rgbtGreen;

                            newry += gy[wi + 1, hw + 1] * rcy;
                            newgy += gy[wi + 1, hw + 1] * gcy;
                            newby += gy[wi + 1, hw + 1] * bcy;
                            //	newr += g[wi + 1,hw + 1] * rcy;
                            //	newg += g[wi + 1,hw + 1] * gcy;
                            //	newb += g[wi + 1,hw + 1] * bcy;
                        }

                    double sum = Math.Sqrt((newb + newg + newr) * (newb + newg + newr) + (newby + newgy + newry) * (newby + newgy + newry));

                    if (sum <= 101)
                    {
                        bitecolorcopy[i, j].rgbtBlue = 0;
                        bitecolorcopy[i, j].rgbtGreen = 0;
                        bitecolorcopy[i, j].rgbtRed = 0;
                    }
                    else
                    {
                        bitecolorcopy[i, j].rgbtBlue = 255;
                        bitecolorcopy[i, j].rgbtGreen = 255;
                        bitecolorcopy[i, j].rgbtRed = 255;
                    }
                }

        }


        public static void DoSobely(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {
            int[,] g = new int[,] {  { -1, 0, 1 }, 
                                     { -2, 0, 2 }, 
                                     { -1, 0, 1 }};

            int[,] gy = new int[,] { { 1, 2, 1 }, 
                                     { 0, 0, 0 }, 
                                     {-1,-2,-1}};


            int newry = 0, newgy = 0, newby = 0;
            int rcy, gcy, bcy;
            int newr = 0, newg = 0, newb = 0;
            int rc, gc, bc;

            for (int i = 1; i <  biHeight - 1; i++)
                for (int j = 1; j <  biWidth - 1; j++)
                {
                    newry = 0; newgy = 0; newby = 0;
                    rcy = 0; gcy = 0; bcy = 0;
                    newr = 0; newg = 0; newb = 0;

                    for (int wi = -1; wi < 2; wi++)
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rcy = bitecolor[i + hw, j + wi].rgbtRed;
                            bcy = bitecolor[i + hw, j + wi].rgbtBlue;
                            gcy = bitecolor[i + hw, j + wi].rgbtGreen;

                            //	newry += gy[wi + 1,hw + 1] * rcy;
                            //	newgy += gy[wi + 1,hw + 1] * gcy;
                            //	newby += gy[wi + 1,hw + 1] * bcy;
                            newr += g[wi + 1, hw + 1] * rcy;
                            newg += g[wi + 1, hw + 1] * gcy;
                            newb += g[wi + 1, hw + 1] * bcy;
                        }

                    double sum = Math.Sqrt((newb + newg + newr) * (newb + newg + newr) + (newby + newgy + newry) * (newby + newgy + newry));

                    if (sum <= 101)
                    {
                        bitecolorcopy[i, j].rgbtBlue = 0;
                        bitecolorcopy[i, j].rgbtGreen = 0;
                        bitecolorcopy[i, j].rgbtRed = 0;
                    }
                    else
                    {
                        bitecolorcopy[i, j].rgbtBlue = 255;
                        bitecolorcopy[i, j].rgbtGreen = 255;
                        bitecolorcopy[i, j].rgbtRed = 255;
                    }
                }

        }
        public static void DoGrey(byte[] data, int biWidth, int biHeight, int biBitCount, Color[,] bitecolor, Color[,] bitecolorcopy)
        {
            int R, G, B, Y;

            for (int i = 0; i <  biHeight; i++)
                for (int j = 0; j <  biWidth; j++)
                {
                    R = bitecolor[i, j].rgbtRed;
                    G = bitecolor[i, j].rgbtGreen;
                    B = bitecolor[i, j].rgbtBlue;
                    Y = (R + G + B) / 3;
                    bitecolor[i, j].rgbtRed = (byte)Y;
                    bitecolor[i, j].rgbtGreen = (byte)Y;
                    bitecolor[i, j].rgbtBlue = (byte)Y;
                }

        }

        public static void CreateBmp(byte[] data, int biWidth, int biHeight, int biBitCount, string[] args, Color[,] bitecolor)
        {
            int k = 54;
            for (int i = 0; i <  biHeight; i++)
            {
                for (int j = 0; j <  biWidth; j++)
                {
                    data[k] = bitecolor[i, j].rgbtRed;
                    k++;
                    data[k] = bitecolor[i, j].rgbtGreen;
                    k++;
                    data[k] = bitecolor[i, j].rgbtBlue;
                    k++;
                    if ( biBitCount == 32) { k++; }
                }
                if ( biBitCount == 24) k +=  biWidth % 4;
            }

            System.IO.File.WriteAllBytes(args[2], data);
        }

        static void Main(string[] args)
        {

        //    FileStream file = new FileStream(@args[0], FileMode.Open, FileAccess.Read);

            byte[] data = System.IO.File.ReadAllBytes(@args[0]);

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
                    if ( biBitCount == 32) { k++; }
                }
                if ( biBitCount == 24) k +=  biWidth % 4;
            }

            string filters = args[1];

            if (filters[0] == '4') DoGrey(data, biWidth, biHeight, biBitCount,  bitecolor, bitecolorcopy);

            if (filters[0] == '0') DoMedian(data, biWidth, biHeight, biBitCount,  bitecolor, bitecolorcopy);

            if (filters[0] == '1') DoGauss(10, data, biWidth, biHeight, biBitCount, bitecolor, bitecolorcopy);

            if (filters[0] == '2') DoSobelx(data, biWidth, biHeight, biBitCount, bitecolor, bitecolorcopy);

            if (filters[0] == '3') DoSobely(data, biWidth, biHeight, biBitCount, bitecolor, bitecolorcopy);

            CreateBmp(data, biWidth, biHeight, biBitCount, args, bitecolorcopy);
            //  createbmp(data, biWidth,  biHeight, biBitCount,, args, bitecolor);
        }
    }
}
