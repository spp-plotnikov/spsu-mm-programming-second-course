using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace FilterClient
{
    class BmpClient
    {
        private int _ownPort;
        private int _abortPort;
        private string _IP;
        private ASCIIEncoding _encoding = new ASCIIEncoding();
        public delegate void statusUpdate(int status);
        public delegate void connectError();
        public BmpClient(string serverIP)
        {

            string[] tmp = serverIP.Split(':');
            _IP = tmp[0];
            _ownPort = Int32.Parse(tmp[1]);
            _abortPort = _ownPort - 1;
        }

        public string[] Init()
        {
            TcpClient client = new TcpClient(_IP, _ownPort);
            NetworkStream io = client.GetStream();

            byte[] buf = new byte[256];
            io.Read(buf, 0, 256);

            io.Close();
            client.Close();

            string[] tmp = _encoding.GetString(buf).Split(' ');
            _ownPort = Int32.Parse(tmp[0]);

            string[] res = new string[tmp.Length - 1];
            Array.Copy(tmp, 1, res, 0, res.Length);

            return res;
        }

        public void Abort ()
        {
            TcpClient client = new TcpClient(_IP, _abortPort);
            NetworkStream io = client.GetStream();

            byte[] bytePort = new byte[32];
            byte[] tmp = _encoding.GetBytes(_ownPort.ToString());
            Array.Copy(tmp, bytePort, tmp.Length);
            io.Write(bytePort, 0, 32);
        }

        public Image Send(string filter, string path, statusUpdate update)
        {
            try
            {
                TcpClient client = new TcpClient(_IP, _ownPort);
                NetworkStream io = client.GetStream();

                Image img = Image.FromFile(path);

                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Bmp);
                    int size = (int)ms.Length;
                    byte[] byteFilterSize = new byte[32];
                    byte[] tmp = _encoding.GetBytes(filter + ' ' + size.ToString());
                    Array.Copy(tmp, byteFilterSize, tmp.Length);
                    io.Write(byteFilterSize, 0, 32);
                    byte[] imgBuf = ms.GetBuffer();
                    io.Write(imgBuf, 0, size);
                }

                int status = 0;
                while (status < 100)
                {
                    int tmp = io.ReadByte();
                    while (tmp == -1)
                    {
                        tmp = io.ReadByte();
                    }
                    status = tmp;
                    update(status);
                }

                Image res = Image.FromStream(io);
                io.Close();
                client.Close();
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
