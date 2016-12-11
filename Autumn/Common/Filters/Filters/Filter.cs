using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Filters
{
    [ServiceContract]
    public class Filter
    {
        private string filterName;
        public delegate Bitmap Mapping(Bitmap srcImage, ref long pixelCounter);
        private Mapping mapping;
        private long pixelCounter;
        private long numOfPixels;
        private int progress;
        
        public Filter(string filterName, Mapping filter)
        {
            this.filterName = filterName;
            mapping = filter;
        }

        public int Progress
        {
            get
            {
                Monitor.Enter(pixelCounter);
                if (numOfPixels == 0) { return 0; }
                {
                   int progress = (int)((double)pixelCounter / numOfPixels * 100);
                }
                Monitor.Exit(pixelCounter);
                return progress;
            }
        }

        public string Name
        {
            get
            {
                return filterName;
            }
        }

        public Bitmap DoFilter(Bitmap srcImage)
        {
            numOfPixels = srcImage.Height * srcImage.Width;
            pixelCounter = 0;
            return mapping(srcImage, ref pixelCounter);
        }
    }
}
