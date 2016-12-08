using ImageConvolutionFilters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        public List<string> filters = new List<string>();
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

        public void ReadFilters()
        {
            Client.GetFilters(filters);
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
            Bitmap result = ApplyFilter.ConvolutionFilter(bytes, filter.Factor, filter.Bias, filter.FilterMatrix);
            Client.GetImage(result);

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
