using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


class TcpTransfer
{
    public void Send(TcpClient socket, byte[] data)
    {
        int bufferSize;

        if (socket.Connected)
            bufferSize = socket.ReceiveBufferSize;
        else
            return;

        byte[] buffer = new byte[bufferSize];
        int dataPointer = 0;

        while (socket.Connected && socket.GetStream().CanWrite && dataPointer < data.Length)
        {
            try
            {
                int curBufferSize = Math.Min(bufferSize, data.Length - dataPointer);
                Array.Copy(data, dataPointer, buffer, 0, curBufferSize);
                socket.GetStream().Write(buffer, 0, curBufferSize);
                dataPointer += curBufferSize;
            }
            catch (Exception)
            {
                continue;
            }
        }
    }

    public byte[] Receive(TcpClient socket)
    {
        byte[] data = new byte[0];

        int bufferSize;

        if (socket.Connected)
            bufferSize = socket.ReceiveBufferSize;
        else
            return data;

        byte[] buffer = new byte[bufferSize];

        int readed = 0;
        do
        {
            try
            {
                if ((readed = socket.GetStream().Read(buffer, 0, buffer.Length)) == 0)
                    continue;
            }
            catch (Exception)
            {
                if (socket.Connected && socket.GetStream().DataAvailable && socket.GetStream().CanRead)
                    continue;
            }

            int shift = data.Length;
            Array.Resize<byte>(ref data, shift + readed);
            Array.Copy(buffer, 0, data, shift, readed);
        }
        while (socket.Connected && socket.GetStream().DataAvailable && socket.GetStream().CanRead);

        return data;
    }

}

