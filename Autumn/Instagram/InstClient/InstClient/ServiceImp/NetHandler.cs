using System;
using System.Net.Sockets;
using System.Text;

namespace ClientShared
{
    public class NetHandler
    {
        private TcpClient _client;
        private readonly string _serverAddress;
        private readonly int _port;

        public NetHandler(string server, int port)
        {
            _serverAddress = server;
            _port = port;
        }

        private byte[] ReceiveResult(int pictSize)
        {

            byte[] receivedBytes = new byte[pictSize];
            _client.GetStream().Read(receivedBytes, 0, pictSize);
            return receivedBytes;
        }

        public void UploadPict(Pict pict, string filt)
        {
            _client = new TcpClient(_serverAddress, _port);

            Upload(Commands.ReceiveFilter);
            Upload(Encoding.UTF8.GetBytes(filt + " "));
            Upload(pict.PictBytes);

            pict.SavePict(ReceiveResult(pict.PictBytes.Length));

            _client.GetStream().Close();
            _client.Close();

        }

        public string[] GetAvalibleFilters()
        {
            _client = new TcpClient(_serverAddress, _port);

            Upload(Commands.GetFilters);

            byte[] recBytes = ReceiveResult(_client.ReceiveBufferSize);

            _client.GetStream().Close();
            _client.Close();

            var filtersString = Encoding.UTF8.GetString(recBytes);

            int filterCount = Int32.Parse(filtersString.Split(' ')[0]);

            string[] result = new string[filterCount];

            Array.Copy(filtersString.Split(' '), 1, result, 0, result.Length);

            return result;

        }

        private void Upload(byte[] uploadingData)
        {
            try
            {

                var stream = _client.GetStream();

                stream.Write(uploadingData, 0, uploadingData.Length);

            }
            catch (Exception)
            {

                throw new Exception("Server is not responding");
            }
        }
    }
}
