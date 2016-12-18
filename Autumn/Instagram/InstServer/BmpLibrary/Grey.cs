namespace BmpLibrary
{
    public static partial class Filters
    {
        public static void Grey(Bmp pict)
        {

            int R, G, B, Y;

            for (int i = 0; i < pict.BiHeight; i++)
            {
                for (int j = 0; j < pict.BiWidth; j++)
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

        }
    }
}
