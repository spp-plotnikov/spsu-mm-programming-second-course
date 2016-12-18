using System;
using System.Threading;

namespace BmpLibrary
{

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs args);

    public class Bmp
    {
        public int BiWidth { get; }
        public int BiHeight { get; }
        public int BiBitCount { get; }
        public event ProgressEventHandler ProgressChanged;

        private byte[] _data;


        public Pixel[,] BitMap { get; set; }

        public Bmp(byte[] data, ProgressEventHandler handler)
        {
            ProgressChanged += handler;
            _data = new byte[data.Length];

            Array.Copy(data, _data, data.Length);

            BiWidth = BitConverter.ToInt32(_data, 18);
            BiHeight = BitConverter.ToInt32(_data, 22);
            BiBitCount = BitConverter.ToInt16(_data, 28);

            BitMap = new Pixel[BiHeight, BiWidth];

            int k = 54;

            for (int i = 0; i < BiHeight; i++)
            {

                for (int j = 0; j < BiWidth; j++)
                {
                    BitMap[i, j].Red = _data[k];
                    k++;

                    BitMap[i, j].Green = _data[k];
                    k++;

                    BitMap[i, j].Blue = _data[k];
                    k++;

                    if (BiBitCount == 32)
                    {
                        k++;
                    }


                }
                if (BiBitCount == 24) k += BiWidth % 4;
            }
            ProgressChanged?.Invoke(this, new ProgressEventArgs(30));
            Thread.Sleep(100);
        }

        public byte[] GetResult()
        {
            int k = 54;

            ProgressChanged?.Invoke(this, new ProgressEventArgs(70));
            Thread.Sleep(100);

            for (int i = 0; i < BiHeight; i++)
            {
                for (int j = 0; j < BiWidth; j++)
                {
                    _data[k] = (byte)BitMap[i, j].Red;
                    k++;

                    _data[k] = (byte)BitMap[i, j].Green;
                    k++;

                    _data[k] = (byte)BitMap[i, j].Blue;
                    k++;

                    if (BiBitCount == 32) { k++; }
                }
                if (BiBitCount == 24) k += BiWidth % 4;
            }

            return _data;
        }

        public Pixel[,] GetBitMapCopy()
        {
            Pixel[,] workArr = new Pixel[BiHeight, BiWidth];

            for (int i = 0; i < BiHeight; i++)
            {
                for (int j = 0; j < BiWidth; j++)
                {
                    workArr[i, j] = BitMap[i, j];
                }
            }

            ProgressChanged?.Invoke(this, new ProgressEventArgs(40));
            return workArr;
        }
    }
}
