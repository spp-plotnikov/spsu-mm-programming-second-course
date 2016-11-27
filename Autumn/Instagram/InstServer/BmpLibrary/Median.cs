using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibrary
{
    public static partial class Filters
    {
        public static void Median(Bmp pict)
        {
            int[] red = new int[9];
            int[] blue = new int[9];
            int[] green = new int[9];


            Pixel[,] workArr = pict.GetBitMapCopy();


            for (int a = 1; a < pict.BiHeight - 1; a++)
            {
                for (int b = 1; b < pict.BiWidth - 1; b++)
                {
                    int k = 0;
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            red[k] = workArr[i + a, j + b].Red;
                            green[k] = workArr[i + a, j + b].Green;
                            blue[k] = workArr[i + a, j + b].Blue;
                            k++;
                        }

                    Array.Sort(red);
                    Array.Sort(green);
                    Array.Sort(blue);

                    pict.BitMap[a, b].Red = red[4];
                    pict.BitMap[a, b].Green = green[4];
                    pict.BitMap[a, b].Blue =blue[4];
                }

            }
             
        }
    }
}
