using System;

namespace BmpLibrary
{
    public static partial class Filters
    {
        public static void Sobel(Bmp pict)
        {

            Pixel[,] workArr = pict.GetBitMapCopy();

            for (int i = 1; i < pict.BiHeight - 1; i++)
            {
                for (int j = 1; j < pict.BiWidth - 1; j++)
                {
                    Pixel x, y, p;

                    x = ((workArr[i - 1, j + 1] + workArr[i - 1, j - 1] * (-1)) +
                        (workArr[i, j + 1] * 2 + workArr[i, j - 1] * (-2)) +
                        (workArr[i + 1, j + 1] + workArr[i + 1, j - 1] * (-1)));

                    y = ((workArr[i + 1, j - 1] + workArr[i - 1, j - 1] * (-1)) +
                        (workArr[i + 1, j] * 2 + workArr[i - 1, j] * (-2)) +
                        (workArr[i + 1, j + 1] + workArr[i + 1, j - 1] * (-1)));


                    p.Blue = (int)Math.Sqrt(x.Blue * x.Blue + y.Blue * y.Blue);
                    p.Red = (int)Math.Sqrt(x.Red * x.Red + y.Red * y.Red);
                    p.Green = (int)Math.Sqrt(x.Green * x.Green + y.Green * y.Green);


                    if (p.Green + p.Red + p.Blue < 128)
                    {
                        pict.BitMap[i, j] = 0;
                    }
                    else
                    {
                        pict.BitMap[i, j] = 255;
                    }

                }

            }

        }
    }
}
