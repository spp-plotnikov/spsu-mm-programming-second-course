using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientFilters
{
    static class ConnectionMethods
    {
        const long sizeOfChunk = 2048;
        private static bool cancelled = false;

        public static bool Cancelled
        {
            get
            {
                return cancelled;
            }
            set
            {
                cancelled = value;
            }
        }

        static public byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        static public Bitmap byteArrayToBitmap(byte[] inputArray)
        {
            Bitmap result = null;
            try
            {
                using (var ms = new System.IO.MemoryStream(inputArray))
                {
                    result = new Bitmap(ms);
                }
            }
            catch { }
            return result;
        }    

        static public void sendByteArrayUsingChunks(byte[] sendArray, Filtering.ServiceClient host)
        {
            host.set_SizeOfSrcArray(sendArray.Length);
            long curBytePosition = 0;
            long remainingBytes = sendArray.Length;
            while ((remainingBytes = sendArray.Length - curBytePosition) > 0 && !cancelled)
            {
                byte[] buffer = new byte[(remainingBytes < sizeOfChunk) ? remainingBytes : sizeOfChunk];
                Array.Copy(sendArray, curBytePosition, buffer, 0, buffer.Length);
                host.ReceiveChunk(buffer, curBytePosition, buffer.Length);
                curBytePosition += buffer.Length;
            }
        }

        static public byte[] receiveByteArrayUsingChunks(Filtering.ServiceClient host)
        {
            long curBytePosition = 0;
            long sizeOfResult = host.get_SizeOfResult();
            byte[] result = new byte[sizeOfResult];
            byte[] buffer = new byte[sizeOfChunk];
            while (curBytePosition < sizeOfResult && !cancelled)
            {
                buffer = host.SendChunk();
                Array.Copy(buffer, 0, result, curBytePosition, (sizeOfResult - curBytePosition >= sizeOfChunk) ? sizeOfChunk : sizeOfResult - curBytePosition);
                curBytePosition += sizeOfChunk;
            }
            return result;
        }      
    }
}
