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
        public delegate Bitmap Mapping(Bitmap srcImage, ref double progress);
        private Mapping mapping;
        private double progress;
        private bool cancelled;
        public bool Cancelled
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


        public Filter(string filterName, Mapping filter)
        {
            this.filterName = filterName;
            mapping = filter;
        }

        public int Progress
        {
            get
            {
                return (int)(progress * 100);
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
            return mapping(srcImage, ref progress);
        }
    }
}
