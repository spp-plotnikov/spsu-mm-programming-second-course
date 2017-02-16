using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Server
{
    class Server
    {
        TcpListener Listener;
        public Server(int port)
        {
            Listener = new TcpListener(IPAddress.Any, port);
            Listener.Start();

            while (true)
            {
                new Client(Listener.AcceptTcpClient());
            }
        }
        class Client
        {
            public Client(TcpClient Client)
            {
                string html = "<html><body><h1>It work!</h1></body></html>";
                string str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Lenght: " + html.Length.ToString() + "\n\n" + html;

                byte[] Buffer = Encoding.ASCII.GetBytes(str);

                Client.GetStream().Write(Buffer, 0, Buffer.Length);
                Client.Close();
            }
        }
        static void Main(string[] args)
        {
            new Server(80);
        }
    }
}
