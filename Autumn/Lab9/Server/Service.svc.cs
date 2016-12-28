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
        readonly Filters _filterWorking;

        public Service()
        {
            _filterWorking = new Filters();
        }

        public byte[] SendImage(byte[] imageArray, string filter)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream = new MemoryStream(imageArray);
            Bitmap image = (Bitmap) Bitmap.FromStream(memoryStream);

            Bitmap newImage= _filterWorking.MakeChanging(image, "Negative");
            
            ImageConverter converter = new ImageConverter();
            byte[] newImageArray = (byte[]) converter.ConvertTo(newImage, typeof(byte[]));
            return newImageArray;
        }
    }
}
