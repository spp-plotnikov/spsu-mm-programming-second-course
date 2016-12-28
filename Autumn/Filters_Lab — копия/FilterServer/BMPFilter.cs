﻿using System;
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
        public static Bitmap ApplyFilter(Bitmap src, string filter, Stream io, ref bool abort)
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
                            if (abort)
                                return null;
                            Color color = src.GetPixel(x, y);
                            int newColor = (color.R + color.G + color.B) / 3;
                            dst.SetPixel(x, y, Color.FromArgb(newColor, newColor, newColor));
                            if (DateTime.Now - startTime >= interval)
                            {
                                startTime = DateTime.Now;
                                double count = (x * src.Height + y + 1) / (double)size;
                                status = (int)(count * 100);
                                try
                                {
                                    io.WriteByte(Convert.ToByte(status));
                                }
                                catch
                                {
                                    abort = true;
                                }
                            }
                        }
                    break;
                
                case "Median":
                    for (int x = 1; x < dst.Width - 1; x++)
                    {
                        for (int y = 1; y < dst.Height - 1; y++)
                        {
                            if (abort)
                                return null;
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
                                try
                                {
                                    io.WriteByte(Convert.ToByte(status));
                                }
                                catch
                                {
                                    abort = true;
                                }
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