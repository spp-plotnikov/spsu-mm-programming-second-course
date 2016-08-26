using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Chat
{
    class Receiver
    {
        public IPEndPoint ReceivingPoint { get; private set; }
        public Socket MainSocket { get; private set; }

        public delegate void DlgForEvent(string message);
        public event DlgForEvent GetSysMsgEvent;

        Thread listenThread;

        public Receiver(int firstPort)
        {
            IPEndPoint ipEndPoint=null;
            IPHostEntry ipHostEntry = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            MainSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            while (true)
            {
                try
                {
                    ipEndPoint = new IPEndPoint(ipAddress, firstPort);
                    MainSocket.Bind(ipEndPoint);
                    ReceivingPoint = ipEndPoint;
                    break;
                }
                catch (Exception)
                {
                    firstPort++;
                }
            }

            listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }


        public void Listen()
        {
            while (true)
            {
                MainSocket.Listen(10);
                Socket listener = MainSocket.Accept();
                string message = null;

                byte[] data = new byte[1024];
                int bytesRec = listener.Receive(data);

                message += Encoding.UTF8.GetString(data, 0, bytesRec);
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();

                if (message == "stop")
                {
                    return;
                }

                if (message.Length > 1)
                {
                    if (message[0] == '@')
                    {
                        message = message.Substring(1);
                        Console.WriteLine(message);
                    }
                    else
                    {
                        GetSysMsgEvent(message);
                    }
                }
            }
        }

        public void StopListening()
        {
            Socket socket = new Socket(MainSocket.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(ReceivingPoint);

            byte[] data = Encoding.UTF8.GetBytes("stop");
            socket.Send(data);
            
            listenThread.Join();
            

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
