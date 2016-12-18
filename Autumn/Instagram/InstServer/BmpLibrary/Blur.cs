namespace BmpLibrary
{
    public static partial class Filters
    {
        public static void Blur(Bmp pict)
        {

            Pixel[,] workArr = pict.GetBitMapCopy();

            for (int i = 1; i < pict.BiHeight - 1; i++)
            {
                for (int j = 1; j < pict.BiWidth - 1; j++)
                {
                    Pixel x;

                    x = ((workArr[i - 1, j + 1] + workArr[i - 1, j - 1] + workArr[i - 1, j]) +
                        (workArr[i, j + 1] + workArr[i, j] + workArr[i, j - 1]) +
                        (workArr[i + 1, j + 1] + workArr[i + 1, j - 1] + workArr[i + 1, j]));

                    x = (x / 9);

                    pict.BitMap[i, j] = x;

                }

            }
        }

    }
}
