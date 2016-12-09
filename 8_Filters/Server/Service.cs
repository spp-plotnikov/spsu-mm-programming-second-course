using ImageConvolutionFilters;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void SendFile(string filterName, Bitmap bytes)
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
            Task<Bitmap> task = Task.Run(() => ApplyFilter.ConvolutionFilter(bytes, filter));
            while(ApplyFilter.Progress <= 100)
            {
                Thread.Sleep(100);
                Client.GetProgress(ApplyFilter.Progress);
                if(ApplyFilter.Progress == 100)
                {
                    Client.GetProgress(ApplyFilter.Progress);
                    ApplyFilter.Progress = 0;
                    break;
                }
            }
            Client.GetImage(task.Result);
        }
        public void SendProgress()
        {
            Client.GetProgress(ApplyFilter.Progress);
          
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
