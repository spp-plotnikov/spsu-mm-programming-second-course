using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTest
{
    class Test
    {
        ServiceReference.ServiceClient client;
        public Test()
        {
            client = new ServiceReference.ServiceClient();
        }

        public long Testing(byte[] bytes)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            byte[] newImage = client.ProceedImage(bytes);
            return watch.ElapsedMilliseconds;
        }
    }
}
