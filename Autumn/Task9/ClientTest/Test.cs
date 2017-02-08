using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class Test
    {
        ServiceReference1.ServiceClient client;
        public Test()
        {
            client = new ServiceReference1.ServiceClient();
        }

        public long Testing(byte[] bytes)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                byte[] newImage = client.ProceedImage(bytes);
            }
            catch
            {
                return -1;
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
