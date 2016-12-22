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
        private string _filterNames;
        private ASCIIEncoding _encoding = new ASCIIEncoding();
        private int _count = 1;
        public BmpServer (int port, string pathToConfig)
        {
            try
            {
                _filterNames = Parse(pathToConfig);
                _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                _server.Start();
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
                Thread newThread = new Thread(() => Listen(newPort));
                newThread.Start();

                byte[] buf = _encoding.GetBytes(newPort.ToString() + ' ' + _filterNames);
                io.Write(buf, 0, buf.Length);

                client.Close();
            }
        }

        private void Listen(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream io = client.GetStream();

                byte[] byteFilterSize = new byte[32];
                io.Read(byteFilterSize, 0, 32);
                string[] tmp = new string[2];
                tmp = _encoding.GetString(byteFilterSize).Split(' ');
                string filter = tmp[0];
                int size = Convert.ToInt32(tmp[1]);
                byte[] bytetmp = new byte[size];

                int sum = 0;
                while (sum < size)
                {
                    sum += io.Read(bytetmp, sum, size - sum);
                }

                Bitmap dst = BMPFilter.ApplyFilter((Bitmap)(new ImageConverter().ConvertFrom(bytetmp)), filter, io);
                using (MemoryStream ms = new MemoryStream())
                {
                    dst.Save(ms, ImageFormat.Bmp);
                    byte[] imgBuf = ms.GetBuffer();
                    io.Write(imgBuf, 0, (int)ms.Length);
                }
                client.Close();
            }
        }
    }
}
