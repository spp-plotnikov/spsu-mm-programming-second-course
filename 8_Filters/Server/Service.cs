using ImageConvolutionFilters;
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
        private List<string> filters = new List<string>();
        public ExtBitmap ApplyFilter = new ExtBitmap();
        private ConvolutionFilterBase filter;
        public int Progress;

        public Service()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("Filters.txt");

            while ((line = file.ReadLine()) != null)
            {
                this.filters.Add(line);
                Console.WriteLine(line);
            }
        }
        
        public List<string> ReadFilters()
        {
            return filters;
        }

        public void SendFile(string filterName, byte[] bytes)
        {

            switch (filterName)
            {
                case "Blur":
                    filter = new BlurFilter();
                    break;
                case "Sharpen":
                    filter = new SharpenFilter();
                    break;
                case "Soften":
                    filter = new SoftenFilter();
                    break;
            }
            ApplyFilter.Cancel = false;
            Bitmap result = new Bitmap(1, 1);
            using (var ms = new MemoryStream(bytes))
            {
                var image = Image.FromStream(ms);
                Task<Bitmap> task = Task.Run(() => ApplyFilter.ConvolutionFilter((Bitmap)image, filter));
                while (ApplyFilter.Progress <= 100 && !ApplyFilter.Cancel)
                {
                    Thread.Sleep(1000);
                    Client.GetProgress(ApplyFilter.Progress);
                    Progress = ApplyFilter.Progress;
                    if (ApplyFilter.Progress == 100)
                    {
                        Client.GetProgress(ApplyFilter.Progress);
                        Progress = ApplyFilter.Progress;
                        ApplyFilter.Progress = 0;
                        break;
                    }
                }
                result = task.Result;
            }

            byte[] data;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                var image = result;
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
                Client.GetImage(data, !ApplyFilter.Cancel);
            
            }
        }
        
        public void Cancel()
        {
            ApplyFilter.Cancel = true;
        }
        public IServiceCallBack Client
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel
                <IServiceCallBack>();
            }
        }
    }
}
