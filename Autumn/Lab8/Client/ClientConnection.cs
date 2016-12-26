using System.Drawing;
using System.IO;

namespace Client
{
    public class ClientConnection : ServiceReference.IServiceCallback
    {
        public int Progress;
        public Bitmap Image;

        public void GetProgress(int progress)
        {
            Progress = progress;
        }

        public void GetImage(byte[] image)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream = new MemoryStream(image);
            Image = (Bitmap)Bitmap.FromStream(memoryStream);
        }
    }
}
