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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    UseSynchronizationContext = true)]
    public class Service : IService
    {
        private bool _isAlive = false;
        private int _progress = 0;
        private Bitmap _image;
        private byte[] _result;
        private int _index;

        // gets the index of the desired filter
        public void SetIndex(int idx)
        {
            _index = idx;
        }

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

        // sends image back to client
        public byte[] GetImage()
        {
            return _result;
        }

        // increments the progress
        private void ProgressIncr()
        {
            _progress++;
        }

        // call for applying a filter
        public byte[] Filter(Bitmap image, int index)
        {
            _index = index;
            _image = image;
            _isAlive = true;
            _result = new byte[image.Width * image.Height * 3];
            switch (_index)
            {
                case 1:
                    {
                        InvertFilter();
                        break;
                    }

                case 2:
                    {
                        SepiaFilter();
                        break;
                    }
            }
            _isAlive = false;
            return _result;
        }

        /// <summary>
        /// Available filters functions:
        /// Inversion
        /// Sepia
        /// </summary>

        private void InvertFilter()
        {
            for (int i = 0; i < _image.Width; i++)
            {
                for (int j = 0; j < _image.Height; j++)
                {
                    Color c = _image.GetPixel(i, j);
                    int red = c.R;
                    int green = c.G;
                    int blue = c.B;
                    red = 255 - red;
                    green = 255 - green;
                    blue = 255 - blue;
                    _result[i * _image.Height * 3 + j * 3] = (byte)red;
                    _result[i * _image.Height * 3 + j * 3 + 1] = (byte)green;
                    _result[i * _image.Height * 3 + j * 3 + 2] = (byte)blue;
                    if (!_isAlive) return;
                }
                _progress = i * 100 / _image.Width;
            }
            _isAlive = false;
            _progress = 100;    
        }

        private void SepiaFilter()
        {
            for (int i = 0; i < _image.Width; i++)
            {
                for (int j = 0; j < _image.Height; j++)
                {
                    Color c = _image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    byte tone = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                    red = (byte)((tone > 206) ? 255 : tone + 49);
                    green = (byte)((tone < 14) ? 0 : tone - 14);
                    blue = (byte)((tone < 56) ? 0 : tone - 56);
                    _result[i * _image.Height * 3 + j * 3] = (byte)red;
                    _result[i * _image.Height * 3 + j * 3 + 1] = (byte)green;
                    _result[i * _image.Height * 3 + j * 3 + 2] = (byte)blue;
                    if (!_isAlive) return;
                }
                _progress = i * 100 / _image.Width;
            }
            _isAlive = false;
            _progress = 100;
            
        }
    }
}
