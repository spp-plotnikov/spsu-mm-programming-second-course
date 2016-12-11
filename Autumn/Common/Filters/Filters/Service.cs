using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Filters
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Multiple,
                 UseSynchronizationContext = true)]
    public class Service : IService
    {
        private Filter chosenFilter;
        private Bitmap srcImage;
        private byte[] byteArray;
        private long curBytePosition;
        private byte[] toSend;
        private long sizeOfChunk = 2048;

        public long SizeOfResult
        {
            get
            {
                return toSend.Length;
            }
        }

        public List<string> Filters()
        {
            return FilterImplementation.FilterNames;
        }



        public int GetProgress()
        {
            return byteArray[255];
        }

        public void DoFilter()
        {
            curBytePosition = 0;
            srcImage = ConnectionMethods.byteArrayToBitmap(byteArray);
            Bitmap result = chosenFilter.DoFilter(srcImage);
            toSend = ConnectionMethods.ImageToByteArray(result);
        }

        public void SetFilter(string filterName)
        {
            chosenFilter = FilterImplementation.FilterList.Find(x => x.Name == filterName);
        }

        public bool SendChunk(byte[] chunk, long fstBytePosition, long sizeOfChunk)
        {
            try
            {
                Array.Copy(chunk, 0, byteArray, fstBytePosition, sizeOfChunk);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetByteArray(long arrayLength)
        {
            this.byteArray = new byte[arrayLength];
        }

        public byte[] GetChunk()
        {
            if (curBytePosition + sizeOfChunk > toSend.Length)
            {
                byte[] buffer = new byte[toSend.Length - curBytePosition];
                Array.Copy(toSend, curBytePosition, buffer, 0, toSend.Length - curBytePosition);
                curBytePosition += sizeOfChunk;
                return buffer;
            }     
            else
            {
                byte[] buffer = new byte[sizeOfChunk];
                Array.Copy(toSend, curBytePosition, buffer, 0, sizeOfChunk);
                curBytePosition += sizeOfChunk;
                return buffer;
            }
        }

        public Bitmap GetResultBitmap()
        {
            return chosenFilter.DoFilter(srcImage);
        }

        
    }
}
