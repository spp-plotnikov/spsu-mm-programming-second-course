using System;

namespace TimeVsClients
{
    public class Client
    {
        ServiceReference.ServiceClient _server;

        public Client()
        {
            _server = new ServiceReference.ServiceClient("NetTcpBinding_IService");
        }

        public long SendAndGetImage(byte[] image)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                byte[] newImage = _server.SendImage(image, "Negative");
            }
            catch (Exception)
            {
                return 0;
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
