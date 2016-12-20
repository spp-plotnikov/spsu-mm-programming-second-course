using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Contracts
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    UseSynchronizationContext = true)]
    public class Service1 : IService1
    {
        private Bitmap _image;
        private int _progress = 0;
        private bool _isAlive;

        public void work()
        {
           // Console.Write("SERVER");
        }

        public List<string> GetListOfFilters()
        {
            List<string> ListOfFilters = new List<string>();
            ListOfFilters.Add("blue");
            ListOfFilters.Add("red");
            ListOfFilters.Add("green");
            return ListOfFilters;
        }

        public Bitmap ApplyFilter(Bitmap image, string nameOfFilter)
        {
            Console.WriteLine(nameOfFilter);
            _isAlive = true;
            _progress = 0;
            _image = new Bitmap(image);// (Bitmap)image.Clone();
            switch (nameOfFilter)
            {
                case "red":
                    Red();
                    break;
                case "blue":
                    Blue();
                    break;
                case "green":
                    Green();
                    break;
            }
            _progress = 100;
            if (_isAlive)
            {
                return _image;
            }
            else
            { 
                return image;
            }
        }


        public int GetProgress()
        {
            Console.WriteLine(_progress);
            return _progress;
        }
        
        public void Stop()
        {
            _isAlive = false;
        }

        private void Red()
        {
            for (int i = 0; i < _image.Width && _isAlive; i++)
            {
                for (int j = 0; j < _image.Height && _isAlive; j++)
                {
                    Color c = _image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    Color newColor = Color.FromArgb((int)(red), (int)(0), (int)(0));
                    _image.SetPixel(i, j, newColor);
                }
                _progress = i * 100 / _image.Width;
                Thread.Sleep(1);
            }
            _progress = 100;
        }

        private void Blue()
        {
            for (int i = 0; i < _image.Width && _isAlive; i++)
            {
                for (int j = 0; j < _image.Height && _isAlive; j++)
                {
                    Color c = _image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    Color newColor = Color.FromArgb((int)(0), (int)(0), (int)(blue));

                    _image.SetPixel(i, j, newColor);
                }
                _progress = i * 100 / _image.Width;
                Thread.Sleep(1);
            }
            _progress = 100;
        }

        private void Green()
        {
            for (int i = 0; i < _image.Width && _isAlive; i++)
            {
                for (int j = 0; j < _image.Height && _isAlive; j++)
                {
                    Color c = _image.GetPixel(i, j);
                    byte red = c.R;
                    byte green = c.G;
                    byte blue = c.B;
                    Color newColor = Color.FromArgb((int)(0), (int)(green), (int)(0));

                    _image.SetPixel(i, j, newColor);
                }
                _progress = i * 100 / _image.Width;
                Thread.Sleep(1);
            }
            _progress = 100;
        }
    }
}
