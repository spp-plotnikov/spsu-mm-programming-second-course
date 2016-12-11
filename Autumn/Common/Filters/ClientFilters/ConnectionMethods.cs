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

        static public byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        static public Bitmap byteArrayToBitmap(byte[] inputArray)
        {
            Bitmap result;
            using (var ms = new System.IO.MemoryStream(inputArray))
            {
                result = new Bitmap(ms);
            }
            return result;
        }    

        static public void sendByteArrayUsingChunks(byte[] sendArray, Filtering.ServiceClient host)
        {
            long curBytePosition = 0;
            long remainingBytes = sendArray.Length;
            while ((remainingBytes = sendArray.Length - curBytePosition) > 0)
            {
                byte[] buffer = new byte[(remainingBytes < sizeOfChunk) ? remainingBytes : sizeOfChunk];
                Array.Copy(sendArray, curBytePosition, buffer, 0, buffer.Length);
                host.SendChunk(buffer, curBytePosition, buffer.Length);
                curBytePosition += buffer.Length;
            }
        }

        static public byte[] receiveByteArrayUsingChunks(Filtering.ServiceClient host)
        {
            long curBytePosition = 0;
            long sizeOfResult = host.get_SizeOfResult();
            byte[] result = new byte[sizeOfResult];
            byte[] buffer = new byte[sizeOfChunk];
            while(curBytePosition < sizeOfResult)
            {
                buffer = host.GetChunk();
                Array.Copy(buffer, 0, result, curBytePosition, (sizeOfResult - curBytePosition >= sizeOfChunk) ? sizeOfChunk : sizeOfResult - curBytePosition);
                curBytePosition += sizeOfChunk;
            }
            return result;
        }      
    }
}
