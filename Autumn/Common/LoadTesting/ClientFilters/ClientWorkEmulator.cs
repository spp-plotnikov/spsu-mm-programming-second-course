using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ClientFilters
{
    class ClientWorkEmulator
    {
        private string path;
        private string nameOfFilter;
        private Bitmap image;

        public ClientWorkEmulator(string path, string nameOfFilter)
        {
            this.path = path;
            this.nameOfFilter = nameOfFilter;
        }

        public ClientWorkEmulator(Bitmap image, string nameOfFilter)
        {
            this.image = image;
            this.nameOfFilter = nameOfFilter;
        }

        public void Start()
        {
            if (image == null) { image = new Bitmap(path); }
            var host = new Filtering.ServiceClient();
            host.SetFilter(nameOfFilter);
            byte[] toSend = ConnectionMethods.ImageToByteArray(image);
            ConnectionMethods.sendByteArrayUsingChunks(toSend, host);
            host.DoFilter();
            byte[] result = ConnectionMethods.receiveByteArrayUsingChunks(host);
            ConnectionMethods.byteArrayToBitmap(result);
        }
    }
}
