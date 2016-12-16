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

        public string[] GetFilters()
        {
            string[] filters = {
            "Фильтр Собеля по X",
            "Фильтр Собеля по Y",
            "Оттенки серого",
            "Размытие по Гауссу"};
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

        public Bitmap GetImage()
        {
            return Image;
        }

        private void ProgressIncr()
        {
            Progress++;
        }

        public void Filter(Bitmap image, int index)
        {
            Image = image;
            IsAlive = true;
            Bitmap newImage = Image;

            switch (index)
            {
                case 1:
                    {
                        fil = 1;
                        Sobel(Image, newImage);
                        break;
                    }
                case 2:
                    {
                        fil = 2;
                        Sobel(Image, newImage);
                        break;
                    }

                case 3:
                    {
                        for (int i = 0; i < Image.Width; i++)
                        {
                            for (int j = 0; j < Image.Height; j++)
                            {
                                if (IsAlive)
                                    GrayScale(i, j, Image, newImage);
                                else
                                {
                                    return;
                                }
                            }
                            ProgressIncr();
                        }
                        break;
                    }
                case 4:
                    {
                        for (int i = 0; i < Image.Width; i++)
                        {
                            for (int j = 0; j < Image.Height; j++)
                            {
                                if (IsAlive)
                                    GaussianBlur(i, j, Image, newImage);
                                else
                                {
                                    return;
                                }
                            }
                            ProgressIncr();
                        }
                        break;
                    }
            }
            Image = newImage;
            IsAlive = false;
        }

        private Move[] offsets = //метод, "бегающий" по элементам матрицы
        {
            new Move(-2,-2), //сдвиг для матрицы 5*5
            new Move(-1,-1) //3*3
        };

        struct Move
        {
            internal int XOffset;
            internal int YOffset;

            public Move(int x, int y)
            {
                XOffset = x;
                YOffset = y;
            }
        }
        private void GrayScale(int col, int row, Bitmap Image, Bitmap newImage)
        {
            byte color = (byte)(((byte)Image.GetPixel(col, row).R + (byte)Image.GetPixel(col, row).G + (byte)Image.GetPixel(col, row).B) / 3);
            newImage.SetPixel(col, row, Color.FromArgb(color, color, color));
        }

        double[,] matGauss = new double[5, 5]
        {
            {0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
            {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            {0.013347, 0.111345, 0.225821, 0.111345, 0.013347},
            {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            {0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
        };

        private void GaussianBlur(int col, int row, Bitmap Image, Bitmap newImage)
        {
            double sR = 0;
            double sG = 0;
            double sB = 0;

            int y = row + offsets[0].YOffset;
            int x = col + offsets[0].XOffset;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int newX = x + j;
                    int newY = y + i;
                    if (newX >= 0 && newY >= 0 && newX < Image.Width && newY < Image.Height)
                    {
                        sR += Image.GetPixel(newX, newY).R * matGauss[i, j];
                        sG += Image.GetPixel(newX, newY).G * matGauss[i, j];
                        sB += Image.GetPixel(newX, newY).B * matGauss[i, j];
                    }
                }
            }

            newImage.SetPixel(col, row, Color.FromArgb((byte)(sR), (byte)(sG), (byte)(sB)));
        }

        int[,] Gx = 
                {
                { -1, -2, -1},
                { 0, 0, 0},
                { 1, 2, 1},
                };

        int[,] Gy = 
                {
                { -1, 0, 1},
                { -2, 0, 2},
                { -1, 0, 1},
                };

        private int fil;

        private void Sobel(Bitmap Image, Bitmap newImage)
        {
            int[,] G = new int[3, 3];

            switch (fil)
            {
                case 1:
                    {
                        G = Gx;
                        break;
                    }
                case 2:
                    {
                        G = Gy;
                        break;
                    }

            }

            int[,] BlackWhite = new int[Image.Width, Image.Height];

            int sum = 0;

            for (int i = 0; i < Image.Width; i++)
            {
                for (int j = 0; j < Image.Height; j++)
                {
                    if (IsAlive)
                        BlackWhite[i, j] = (Image.GetPixel(i, j).R + Image.GetPixel(i, j).G + Image.GetPixel(i, j).B) / 3;
                    else
                    {
                        return;
                    }
                }
                if (i % 2 == 0)
                {
                    ProgressIncr();
                }
            }

            for (int col = 0; col < Image.Width; col++)
            {
                for (int row = 0; row < Image.Height; row++)
                {

                    int y = row + offsets[1].YOffset;
                    int x = col + offsets[1].XOffset;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (IsAlive)
                            {
                                int newX = x + j;
                                int newY = y + i;
                                if (newX >= 0 && newY >= 0 && newX < Image.Width && newY < Image.Height)
                                {
                                    sum += BlackWhite[newX, newY] * G[i, j];
                                }
                            }
                            else
                            {
                                return;
                            }

                        }
                    }
                    newImage.SetPixel(col, row, Color.FromArgb((byte)Math.Sqrt(sum * sum), (byte)Math.Sqrt(sum * sum), (byte)Math.Sqrt(sum * sum)));
                    sum = 0;
                }
                if (col % 2 == 0)
                {
                    ProgressIncr();
                }
            }
        }
    }
}
