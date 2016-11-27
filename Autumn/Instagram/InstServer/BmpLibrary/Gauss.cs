using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibrary
{
    public static partial class Filters
    {
        public static void Gauss(Bmp pict)
        {

            Pixel[,] workArr = pict.GetBitMapCopy();

            for (int i = 1; i < pict.BiHeight - 1; i++)
            {
                for (int j = 1; j < pict.BiWidth - 1; j++)
                {
                    Pixel x;

                    x = ((workArr[i - 1, j + 1] + workArr[i - 1, j - 1] + workArr[i - 1, j] * 2) +
                        (workArr[i, j + 1] * 2 + workArr[i, j] * 4 + workArr[i, j - 1] * 2) +
                        (workArr[i + 1, j + 1] + workArr[i + 1, j - 1] + workArr[i + 1, j] * 2));

                    x = (x / 16);

                    pict.BitMap[i, j] = x;

                }

            }
        }

    }
}
