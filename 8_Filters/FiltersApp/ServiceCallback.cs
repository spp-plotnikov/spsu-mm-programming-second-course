using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;

namespace Server
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class MyCallback : IServiceCallBack
    {
        public List<string> Filters = new List<string>();
        public byte[] Result = new byte[200000000];
        public int Progress = 0;
        public bool ImageHere = false;

        public void GetFilters(List<string> filters)
        {
            this.Filters = filters;
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
            Result = data;
            ImageHere = true;
        }

        public void GetProgress(int progress)
        {
            Progress = progress;
        }
    }
}
