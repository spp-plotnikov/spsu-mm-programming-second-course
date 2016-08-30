using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat
{
    class Sender
    {
        public List<IPEndPoint> IPEndPointList { get; private set; }
        public Socket MainSocket { get; private set; }
        int ownPort;
        IPAddress ownIP;

        public Sender(Receiver receiver, int firstPort)
        {
            ownIP = receiver.ReceivingPoint.Address;
            ownPort = receiver.ReceivingPoint.Port;

            IPEndPointList = new List<IPEndPoint>();
            MainSocket = new Socket(ownIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            MainSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);


            GetFirstAdressPool(firstPort, ownPort);
            SendEndPointInfo(ownIP, ownPort, true, 2);

            receiver.GetSysMsgEvent += SysMessage;
        }

        void GetFirstAdressPool(int port, int ownPort)
        {
            IPEndPoint ipEndPoint = null;
            IPHostEntry ipHostEntry = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostEntry.AddressList[0];

            for (int curPort = port; curPort < port + 10; curPort++)
            {
                try
                {

                    Socket socket = new Socket(MainSocket.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    ipEndPoint = new IPEndPoint(ipAddress, curPort);
                    socket.Bind(ipEndPoint);
                    socket.Close();
                }
                catch (Exception)
                {
                    if (curPort != ownPort)
                        IPEndPointList.Add(ipEndPoint);
                }
            }
        }

        void SendEndPointInfo(IPAddress iP, int port, bool onlyToFirstValid, int repeat)
        {
            Send(Encoding.UTF8.GetString(iP.GetAddressBytes()) + ' ' + port.ToString()
                + ' ' + repeat.ToString(), onlyToFirstValid);
        }

        public void Send(string message, bool onlyToFirstValid)
        {
            List<int> EndPtsWithoutListen = new List<int>();
            int count = 0;
            try
            {
                AdjustSend(message, onlyToFirstValid, EndPtsWithoutListen, count);
            }
            catch (InvalidOperationException)
            {
                count = 0;
                EndPtsWithoutListen.Clear();
                //Console.WriteLine("Something wrong with connect, try again");
                AdjustSend(message, onlyToFirstValid, EndPtsWithoutListen, count);

            }

        }

        public void SysMessage(string message)
        {
            try
            {
                string[] args = message.Split(' ');
                string ip = args[0];
                IPAddress receivedIp = new IPAddress(Encoding.UTF8.GetBytes(ip));
                int receivedPort = int.Parse(args[1]);
                int repeat = int.Parse(args[2]);

                IPEndPoint ipEndPoint = new IPEndPoint(receivedIp, receivedPort);

                if (!IPEndPointList.Contains(ipEndPoint) && ipEndPoint.Port != ownPort)
                    IPEndPointList.Add(ipEndPoint);

                if (repeat == 1)
                {
                    IPEndPointList.Reverse();
                    SendEndPointInfo(ownIP, ownPort, true, repeat - 1);
                }

                if (repeat == 2)
                    SendEndPointInfo(receivedIp, receivedPort, false, repeat - 1);
            }

            catch (Exception)
            {
                Console.WriteLine("Troubles with SysMsgHandler");
            }
        }

        private void AdjustSend(string message, bool onlyToFirstValid,
            List<int> EndPtsWithoutListen, int count)
        {
            #region
            foreach (IPEndPoint ipEndPoint in IPEndPointList)
            {
                try
                {
                    Socket socket = new Socket(MainSocket.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    socket.Connect(ipEndPoint);

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data);

                    if (onlyToFirstValid)
                        break;

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception)
                {
                    EndPtsWithoutListen.Add(count);
                }
                count++;
            }

            count = 0;
            foreach (int pos in EndPtsWithoutListen)
            {
                IPEndPointList.RemoveAt(pos - count);
                count++;
            }
            #endregion
        }

    }
}
