using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Filters
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    //             ConcurrencyMode = ConcurrencyMode.Multiple,
    //             UseSynchronizationContext = true)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]

    public class Service : IService
    {
        private Filter chosenFilter;
        private Bitmap srcImage;
        private byte[] recievedArray;
        private long curBytePosition;
        private byte[] sendArray;
        private long sizeOfChunk = 2048;
        private bool cancelled = false;
        
        public long SizeOfResult
        {
            get
            {
                return sendArray.Length;
            }
        }

        public long SizeOfSrcArray
        {
            set
            {
                recievedArray = new byte[value];
            }
        }

        public void Cancel()
        {
            cancelled = true;
        }

        public List<string> Filters()
        {
            List<string> result = new List<string>();
            foreach (Filter filter in FiltersWImplementation.filterList)
            {
                result.Add(filter.Name);
            }
            return result;
        }
        
        public int GetProgress()
        {
            return chosenFilter.Progress;
        }

        public void DoFilter()
        {
            cancelled = false;
            curBytePosition = 0;
            srcImage = ConnectionMethods.byteArrayToBitmap(recievedArray);
            Bitmap result = chosenFilter.DoFilter(srcImage);
            sendArray = ConnectionMethods.ImageToByteArray(result);
        }

        public void SetFilter(string filterName)
        {
            chosenFilter = FiltersWImplementation.ListOfFilters.Find(x => x.Name == filterName);
        }

        public bool ReceiveChunk(byte[] chunk, long fstBytePosition, long sizeOfChunk) // Server recieve chunk from Client
        {
            try
            {
                Array.Copy(chunk, 0, recievedArray, fstBytePosition, sizeOfChunk);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public byte[] SendChunk()
        {
            if (curBytePosition + sizeOfChunk > sendArray.Length)
            {
                byte[] buffer = new byte[sendArray.Length - curBytePosition];
                Array.Copy(sendArray, curBytePosition, buffer, 0, sendArray.Length - curBytePosition);
                curBytePosition += sizeOfChunk;
                return buffer;
            }     
            else
            {
                byte[] buffer = new byte[sizeOfChunk];
                Array.Copy(sendArray, curBytePosition, buffer, 0, sizeOfChunk);
                curBytePosition += sizeOfChunk;
                return buffer;
            }
        }                   
    }
}
