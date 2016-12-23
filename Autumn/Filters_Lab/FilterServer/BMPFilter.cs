using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FilterServer
{
    static class BMPFilter
    {
        public delegate void update(int status);
        public static Bitmap ApplyFilter(Bitmap src, string filter, Stream io)
        {
            Bitmap dst = (Bitmap)src.Clone();
            
            DateTime startTime = DateTime.Now;
            int status = 0;
            long size = src.Width * src.Height;
            TimeSpan interval = new TimeSpan(10000000);
            switch (filter)
            {
                case "Grey":
                    for (int x = 0; x < dst.Width; x++)
                        for (int y = 0; y < dst.Height; y++)
                        {
                            Color color = src.GetPixel(x, y);
                            int newColor = (color.R + color.G + color.B) / 3;
                            dst.SetPixel(x, y, Color.FromArgb(newColor, newColor, newColor));
                            if (DateTime.Now - startTime >= interval)
                            {
                                startTime = DateTime.Now;
                                double count = (x * src.Height + y + 1) / (double)size;
                                status = (int)(count * 100);
                                io.WriteByte(Convert.ToByte(status));
                            }
                        }
                    break;
                case "Gauss":
                    int[,] mask = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };

                    for (int x = 1; x < dst.Width - 1; x++)
                        for (int y = 1; y < dst.Height - 1; y++)
                        {
                            int newR = 0, newG = 0, newB = 0;
                            for (int dx = -1; dx <= 1; dx++)
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    Color curColor = src.GetPixel(x + dx, y + dy);
                                    newR += curColor.R * mask[1 + dx, 1 + dy];
                                    newG += curColor.G * mask[1 + dx, 1 + dy];
                                    newB += curColor.B * mask[1 + dx, 1 + dy];
                                }
                            dst.SetPixel(x, y, Color.FromArgb(newR/16, newG/16, newB/16));
                            if (DateTime.Now - startTime >= interval)
                            {
                                startTime = DateTime.Now;
                                double count = (x * src.Height + y + 1) / (double)size;
                                status = (int)(count * 100);
                                io.WriteByte(Convert.ToByte(status));
                            }
                        }
                    break;
                case "Sobel":
                    int[,] maskX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                    int[,] maskY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
                    int min = 200;
                    for (int x = 1; x < dst.Width - 1; x++)
                        for (int y = 1; y < dst.Height - 1; y++)
                        {
                            int resX = 0, resY = 0;
                            for (int dx = -1; dx <= -1; dx++)
                                for (int dy = -1; dy <= -1; dy++)
                                {
                                    Color curColor = src.GetPixel(x + dx, y + dy);
                                    int tmp = curColor.R + curColor.G + curColor.B;
                                    resX += tmp / 3 * maskX[dx + 1, dy + 1];
                                    resY += tmp / 3 * maskY[dx + 1, dy + 1];
                                }
                            if (Math.Sqrt(resX * resX + resY * resY) > min)
                                dst.SetPixel(x, y, Color.White);
                            else
                                dst.SetPixel(x, y, Color.Black);
                            if (DateTime.Now - startTime >= interval)
                            {
                                startTime = DateTime.Now;
                                double count = (x * src.Height + y + 1) / (double)size;
                                status = (int)(count * 100);
                                io.WriteByte(Convert.ToByte(status));
                            }
                        }
                    break;
                case "Median":
                    for (int x = 1; x < dst.Width - 1; x++)
                    {
                        for (int y = 1; y < dst.Height - 1; y++)
                        {
                            int[] R = new int[9];
                            int[] G = new int[9];
                            int[] B = new int[9];

                            for (int dx = -1; dx <= 1; dx++)
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    Color curColor = src.GetPixel(x + dx, y + dy);
                                    R[(dx + 1) * 3 + dy + 1] = curColor.R;
                                    G[(dx + 1) * 3 + dy + 1] = curColor.G;
                                    B[(dx + 1) * 3 + dy + 1] = curColor.B;
                                }

                            Array.Sort(R);
                            Array.Sort(G);
                            Array.Sort(B);

                            dst.SetPixel(x, y, Color.FromArgb(R[4], G[4], B[4]));

                            if (DateTime.Now - startTime >= interval)
                            {
                                startTime = DateTime.Now;
                                double count = (x * src.Height + y + 1) / (double)size;
                                status = (int)(count * 100);
                                io.WriteByte(Convert.ToByte(status));
                            }
                        }
                    }
                    break;
            }
            if (status != 100)
                io.WriteByte(Convert.ToByte(100));

            return dst;

        }

    }
}
