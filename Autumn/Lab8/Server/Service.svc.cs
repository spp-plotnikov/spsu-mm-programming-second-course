using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        readonly List<string> _filters;
        readonly Filters _filterWorking;
        readonly IServiceCallback _client;

        public Service()
        {
            _client = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            
            _filterWorking = new Filters();
            _filters = new List<string> { "Negative", "Gray", "Contrast" };
        }

        public string[] GetFilters()
        {
            return _filters.ToArray();
        }

        public void SendImage(byte[] imageArray, string filter)
        {
            _filterWorking.CancelProcess = false;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream = new MemoryStream(imageArray);
            Bitmap image = (Bitmap)Bitmap.FromStream(memoryStream);

            _client.GetProgress(_filterWorking.Progress);
            Task<Bitmap> newImage = Task.Run(() => _filterWorking.MakeChanging(image, filter));

            while (_filterWorking.Progress != 75)
            {
                Thread.Sleep(100);
                _client.GetProgress(_filterWorking.Progress);
            }
            ImageConverter converter = new ImageConverter();
            byte[] newImageArray = (byte[])converter.ConvertTo(newImage.Result, typeof(byte[]));

            _client.GetImage(newImageArray);
        }

        public void SendCancel(bool cancel)
        {
            _filterWorking.CancelProcess = cancel;
        }
    }
}
