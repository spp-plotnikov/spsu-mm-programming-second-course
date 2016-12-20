using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;
using System.Drawing;
using System.ServiceModel;
namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        private bool _isAlive = false;
        private int _progress = 0;
        private int _index;
        byte[] _res;

        // sends list of filters to client
        public string[] GetFilters()
        {
            string[] filters =
            {
                "Инвертирование",
                "Сепия",
            };
            return filters;
        }

        // Sets progress to zero
        public void CancelProgress()
        {
            _progress = 0;
        }

        // checks the status of process
        public bool CheckIsAlive()
        {
            return _isAlive;
        }
        public byte[] GetImage()
        {
            return _res;
        }

        // changes the status of process
        public void ChangeIsAlive(bool x)
        {
            _isAlive = x;
        }

        // sends progress to client
        public int GetProgress()
        {
            return _progress;
        }

        public void Clear()
        {
            _res = null;
            GC.Collect();
        }

        // call for applying a filter
        public void Filter(Bitmap image, int index)
        {
            _index = index;
            _isAlive = true;
            _res = new byte[image.Width * image.Height * 3];
            Bitmap imag = new Bitmap(image);
            switch (_index)
            {
                case 1:
                    {
                        _res = InvertFilter(imag);
                        break;
                    }

                case 2:
                    {
                        _res = SepiaFilter(imag);
                        break;
                    }
            }
            return;
        }

        /// <summary>
        /// Available filters functions:
        /// Inversion
        /// Sepia
        /// </summary>
        byte[] InvertFilter(Bitmap image)
        {
            byte[] result = new byte[image.Width * image.Height * 3];
            for (int i = 0; i < image.Width && _isAlive; i++)
            {
                for (int j = 0; j < image.Height && _isAlive; j++)
                {
                    Color c = image.GetPixel(i, j);
                    int red = c.R;
                    int green = c.G;
                    int blue = c.B;
                    red = 255 - red;
                    green = 255 - green;
                    blue = 255 - blue;
                    result[i * image.Height * 3 + j * 3] = (byte)red;
                    result[i * image.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * image.Height * 3 + j * 3 + 2] = (byte)blue;
                }
                _progress = i * 100 / image.Width;
                Thread.Sleep(10);
            }
            if (_isAlive) _progress = 100;
            return result;
        }

        byte[] SepiaFilter(Bitmap image)
        {
            byte[] result = new byte[image.Width * image.Height * 3];
            for (int i = 0; i < image.Width && _isAlive; i++)
            {
                for (int j = 0; j < image.Height && _isAlive; j++)
                {
                    Color c = image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte tone = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    red = (byte)((tone > 206) ? 255 : tone + 49);
                    green = (byte)((tone < 14) ? 0 : tone - 14);
                    blue = (byte)((tone < 56) ? 0 : tone - 56);
                    result[i * image.Height * 3 + j * 3] = (byte)red;
                    result[i * image.Height * 3 + j * 3 + 1] = (byte)green;
                    result[i * image.Height * 3 + j * 3 + 2] = (byte)blue;
                }
                _progress = i * 100 / image.Width;
                Thread.Sleep(10);
            }
            if (_isAlive) _progress = 100;
            return result;
        }
    }
}
