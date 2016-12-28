namespace TimeVsImage
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
            byte[] newImage = _server.SendImage(image, "Negative");
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}