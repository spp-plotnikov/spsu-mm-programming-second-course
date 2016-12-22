using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    struct RGB
    {
        public float R;
        public float G;
        public float B;
    }

    class Filters
    {
        public static UInt32[,] matrix_filtration(int W, int H, UInt32[,] pixel, int N, double[,] matryx)
        {
            int i, j, k, m, gap = (int)(N / 2);
            int tmpH = H + 2 * gap, tmpW = W + 2 * gap;
            UInt32[,] tmppixel = new UInt32[tmpH, tmpW];
            UInt32[,] newpixel = new UInt32[H, W];
            //заполнение временного расширенного изображения
            //углы 
            for (i = 0; i < gap; i++)
                for (j = 0; j < gap; j++)
                {
                    tmppixel[i, j] = pixel[0, 0];
                    tmppixel[i, tmpW - 1 - j] = pixel[0, W - 1];
                    tmppixel[tmpH - 1 - i, j] = pixel[H - 1, 0];
                    tmppixel[tmpH - 1 - i, tmpW - 1 - j] = pixel[H - 1, W - 1];
                }
            //крайние левая и правая стороны
            for (i = gap; i < tmpH - gap; i++)
                for (j = 0; j < gap; j++)
                {
                    tmppixel[i, j] = pixel[i - gap, j];
                    tmppixel[i, tmpW - 1 - j] = pixel[i - gap, W - 1 - j];
                }
            //крайние верхняя и нижняя стороны
            for (i = 0; i < gap; i++)
                for (j = gap; j < tmpW - gap; j++)
                {
                    tmppixel[i, j] = pixel[i, j - gap];
                    tmppixel[tmpH - 1 - i, j] = pixel[H - 1 - i, j - gap];
                }
            //центр
            for (i = 0; i < H; i++)
                for (j = 0; j < W; j++)
                    tmppixel[i + gap, j + gap] = pixel[i, j];
            //применение ядра свертки
            RGB colorOfPixel = new RGB();
            RGB colorOfCell = new RGB();
            for (i = gap; i < tmpH - gap; i++)
                for (j = gap; j < tmpW - gap; j++)
                {
                    colorOfPixel.R = 0;
                    colorOfPixel.G = 0;
                    colorOfPixel.B = 0;
                    for (k = 0; k < N; k++)
                        for (m = 0; m < N; m++)
                        {
                            colorOfCell = calculationOfColor(tmppixel[i - gap + k, j - gap + m], matryx[k, m]);
                            colorOfPixel.R += colorOfCell.R;
                            colorOfPixel.G += colorOfCell.G;
                            colorOfPixel.B += colorOfCell.B;
                        }
                    //контролируем переполнение переменных
                    if (colorOfPixel.R < 0) colorOfPixel.R = 0;
                    if (colorOfPixel.R > 255) colorOfPixel.R = 255;
                    if (colorOfPixel.G < 0) colorOfPixel.G = 0;
                    if (colorOfPixel.G > 255) colorOfPixel.G = 255;
                    if (colorOfPixel.B < 0) colorOfPixel.B = 0;
                    if (colorOfPixel.B > 255) colorOfPixel.B = 255;

                    newpixel[i - gap, j - gap] = build(colorOfPixel);
                }

            return newpixel;
        }

        //вычисление нового цвета
        public static RGB calculationOfColor(UInt32 pixel, double coefficient)
        {
            RGB Color = new RGB();
            Color.R = (float)(coefficient * ((pixel & 0x00FF0000) >> 16));
            Color.G = (float)(coefficient * ((pixel & 0x0000FF00) >> 8));
            Color.B = (float)(coefficient * (pixel & 0x000000FF));
            return Color;
        }

        //сборка каналов
        public static UInt32 build(RGB colorOfPixel)
        {
            UInt32 Color;
            Color = 0xFF000000 | ((UInt32)colorOfPixel.R << 16) | ((UInt32)colorOfPixel.G << 8) | ((UInt32)colorOfPixel.B);
            return Color;
        }


        //повышение резкости
        public const int N1 = 3;
        public static double[,] sharpness = new double[N1, N1] {{-1, -1, -1},
                                                               {-1,  9, -1},
                                                               {-1, -1, -1}};

        //размытие
        public const int N2 = 3;
        public static double[,] blur = new double[N1, N1] {{0.111, 0.111, 0.111},
                                                               {0.111, 0.111, 0.111},
                                                               {0.111, 0.111, 0.111}};
    }
}
