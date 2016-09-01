using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibrary
{
    public static class Filter
    {
        public static void Grey(string pathToBmp, string pathToSave)
        {
            BmpLib pict = new BmpLib(pathToBmp);

            int R, G, B, Y;

            for (int i = 0; i < pict.biHeight; i++)
            {
                for (int j = 0; j < pict.biWidth; j++)
                {
                    R = pict.BitMap[i, j].Red;
                    G = pict.BitMap[i, j].Green;
                    B = pict.BitMap[i, j].Blue;

                    Y = (R + G + B) / 3;
                    pict.BitMap[i, j].Red = (byte)Y;
                    pict.BitMap[i, j].Green = (byte)Y;
                    pict.BitMap[i, j].Blue = (byte)Y;
                }
            }

            pict.CreateBmp(pathToSave);
        }
    }
}
