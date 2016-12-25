using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace FilterServer
{
    class BmpServer
    {
        private TcpListener _server;
        private TcpListener _abortServer;
        private string _filterNames;
        private ASCIIEncoding _encoding = new ASCIIEncoding();
        private int _count = 1;
        private Dictionary<int, bool[]> _abortPool = new Dictionary<int, bool[]>();
        public BmpServer (int port, string pathToConfig)
        {
            try
            {
                _filterNames = Parse(pathToConfig);
                _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                _abortServer = new TcpListener(IPAddress.Parse("127.0.0.1"), port-1);
                _server.Start();
                _abortServer.Start();
                Console.WriteLine("Filters: " + _filterNames);
                Thread abort = new Thread(() => RunAbort());
                abort.Start();

                Run();
            }
            catch
            {
                Console.WriteLine("Couldn't open file / create connection");
                Console.WriteLine("Press 'Enter' to exit");
                Console.ReadKey();
            }
        }

        private string Parse (string pathToConfig)
        {
            return new StreamReader(new FileStream(pathToConfig, FileMode.Open)).ReadToEnd();
        }

        private void Run ()
        {
            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
                NetworkStream io = client.GetStream();

                int newPort = 13000 + _count++;
                bool[] abort = { false };
                _abortPool.Add(newPort, abort);

                Thread newThread = new Thread(() => Listen(newPort));
                newThread.Start();

                byte[] buf = _encoding.GetBytes(newPort.ToString() + ' ' + _filterNames);
                io.Write(buf, 0, buf.Length);

                io.Close();
                client.Close();
            }
        }

        private void RunAbort ()
        {
            while (true)
            {
                TcpClient client = _abortServer.AcceptTcpClient();
                NetworkStream io = client.GetStream();

                byte[] bytePort = new byte[32];
                io.Read(bytePort, 0, 32);
                string tmp = _encoding.GetString(bytePort);

                int port = Int32.Parse(tmp);
                _abortPool[port][0] = true;
            }
        }

        public void Listen(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream io = client.GetStream();

                try
                {
                    byte[] byteFilterSize = new byte[32];
                    io.Read(byteFilterSize, 0, 32);
                    string[] tmp = new string[2];
                    tmp = _encoding.GetString(byteFilterSize).Split(' ');
                    string filter = tmp[0];
                    int size = Int32.Parse(tmp[1]);

                    byte[] bytetmp = new byte[size];
                    int sum = 0;
                    while (sum < size)
                    {
                        sum += io.Read(bytetmp, sum, size - sum);
                    }

                    Bitmap dst = BMPFilter.ApplyFilter((Bitmap)(new ImageConverter().ConvertFrom(bytetmp)), filter, io, ref _abortPool[port][0]);
                    if (!_abortPool[port][0])
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dst.Save(ms, ImageFormat.Bmp);
                            byte[] imgBuf = ms.GetBuffer();
                            io.Write(imgBuf, 0, (int)ms.Length);
                        }
                    }
                    else
                    {
                        _abortPool[port][0] = false;
                    }
                }
                finally
                {
                    io.Close();
                    client.Close();
                }
            }
        }
    }
}
