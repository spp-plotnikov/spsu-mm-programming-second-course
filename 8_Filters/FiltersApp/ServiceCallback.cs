using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;

namespace Server
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class MyCallBack : IServiceCallBack
    {
        public List<string> Filters = new List<string>();
        public byte[] Result = new byte[200000000];
        public int Progress = 0;

        public void RequestFilters()
        {
            Server.ReadFilters();
        }
        public void GetFilters(List<string> filters)
        {
            this.Filters = filters;
        }

        public void RequestImage(string filterName, Bitmap source)
        {
            byte[] result = new byte[200000000];
            Server.SendFile(filterName, source);
        }
        public void GetImage(Bitmap image)
        {
            byte[] data;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
            }
            this.Result = data;
        }

        public void RequestProgress()
        {

            Server.SendProgress();
        }
        public void GetProgress(int progress)
        {
            this.Progress = progress;
        }
        public void Cancel()
        {
            Server.Cancel();
        }
        public IService Server
        {
            get
            {
                var context = new InstanceContext(this);
                return new DuplexChannelFactory<IService>(context, "WSDualHttpBinding_INotificationServices").CreateChannel();
            }
        }

    }
}
