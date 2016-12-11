﻿/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImageConvolutionFilters
{
    public  class ExtBitmap

    {
        public  int Progress;
        public bool Cancel;
        public   Bitmap ConvolutionFilter(Bitmap sourceBitmap, ConvolutionFilterBase filter) 
                                       
        {
            Progress = 0;
            Cancel = false;
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                            sourceBitmap.Width, sourceBitmap.Height),
                                            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
 
            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filter.FilterMatrix.GetLength(1);
            int filterHeight = filter.FilterMatrix.GetLength(0);

            int filterOffset = (filterWidth-1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;
            long size = (sourceBitmap.Width - 2 * filterOffset) * (sourceBitmap.Height - 2 * filterOffset);
            
            for (int offsetY = filterOffset; offsetY < 
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    byteOffset = offsetY *
                                sourceData.Stride +
                                 offsetX * 4;
                    if (Cancel)
                    {
                        break;
                    }
                    Progress = (int)(((offsetY - filterOffset) * (sourceBitmap.Width - 2 * filterOffset)) / (double)size * 100);
                    Console.WriteLine(Progress); //make this method slower to see the progress on my form
                    for (int filterY = -filterOffset; 
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {

                            calcOffset = byteOffset + 
                                         (filterX * 4) + 
                                         (filterY * sourceData.Stride);

                            blue += (double)(pixelBuffer[calcOffset]) * 
                                    filter.FilterMatrix[filterY + filterOffset, 
                                                        filterX + filterOffset];

                            green += (double)(pixelBuffer[calcOffset + 1]) * 
                                     filter.FilterMatrix[filterY + filterOffset, 
                                                        filterX + filterOffset];

                            red += (double)(pixelBuffer[calcOffset + 2]) * 
                                   filter.FilterMatrix[filterY + filterOffset, 
                                                      filterX + filterOffset];
                        }
                    }

                    blue = filter.Factor * blue + filter.Bias;
                    green = filter.Factor * green + filter.Bias;
                    red = filter.Factor * red + filter.Bias;

                    if (blue > 255)
                    { blue = 255; }
                    else if (blue < 0)
                    { blue = 0; }

                    if (green > 255)
                    { green = 255; }
                    else if (green < 0)
                    { green = 0; }

                    if (red > 255)
                    { red = 255; }
                    else if (red < 0)
                    { red = 0; }

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;                  
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                     ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            Progress = 100; //because of actions with double
            return resultBitmap;
        }
    }  
}
