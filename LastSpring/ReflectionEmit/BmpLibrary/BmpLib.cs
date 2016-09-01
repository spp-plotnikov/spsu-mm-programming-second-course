using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpLibrary
{
    public class BmpLib
    {
        public int biWidth
        {
            get;
            private set;
        }
        public int biHeight
        {
            get;
            private set;
        }
        public int biBitCount
        {
            get;
            private set;
        }
        private byte[] _data;

        public Pixel[,] BitMap { get; set; }

        public BmpLib(string path)
        {

            byte[] data = System.IO.File.ReadAllBytes(path);

            _data = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                _data[i] = data[i];
            }

            biWidth = BitConverter.ToInt32(_data, 18);
            biHeight = BitConverter.ToInt32(_data, 22);
            biBitCount = BitConverter.ToInt16(_data, 28);

            BitMap = new Pixel[biHeight, biWidth];

            int k = 54;

            for (int i = 0; i < biHeight; i++)
            {

                for (int j = 0; j < biWidth; j++)
                {
                    BitMap[i, j].Red = _data[k];
                    k++;

                    BitMap[i, j].Green = _data[k];
                    k++;

                    BitMap[i, j].Blue = _data[k];
                    k++;

                    if (biBitCount == 32)
                    {
                        k++;
                    }


                }
                if (biBitCount == 24) k += biWidth % 4;
            }
        }

        public void CreateBmp(string pathAndName)
        {
            int k = 54;
            for (int i = 0; i < biHeight; i++)
            {
                for (int j = 0; j < biWidth; j++)
                {
                    _data[k] = BitMap[i, j].Red;
                    k++;

                    _data[k] = BitMap[i, j].Green;
                    k++;

                    _data[k] = BitMap[i, j].Blue;
                    k++;

                    if (biBitCount == 32) { k++; }
                }
                if (biBitCount == 24) k += biWidth % 4;
            }

            System.IO.File.WriteAllBytes(pathAndName, _data);
        }
    }
}
