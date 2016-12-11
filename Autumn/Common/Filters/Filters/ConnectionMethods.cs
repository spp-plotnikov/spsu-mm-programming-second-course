using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    static class ConnectionMethods
    {
        static public byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        static public Bitmap byteArrayToBitmap(byte[] inputArray)
        {
            return (Bitmap)((new ImageConverter()).ConvertFrom(inputArray));
        }
                
    }
}
