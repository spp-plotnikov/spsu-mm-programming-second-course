
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Web.Script.Serialization;
using System.Drawing;
using Fleck;


namespace WebServer
{
    class Server
    {
        private TcpListener listener; 
       
        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port); 
            listener.Start(); 

            // Mian loop
            while (true)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), listener.AcceptTcpClient());
            }
        }

        private void ClientThread(Object stateInfo)
        {  
            new Client((TcpClient)stateInfo);
        }


        private static void processingImg(IWebSocketConnection socket, string requestRAW)
        {
            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();

            var decodedRequest = new Dictionary<string, string>();
            try
            {
                decodedRequest = jsonConvert.Deserialize<Dictionary<string, string>>(requestRAW);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                socket.Send("error");
                return;
            }


            if (decodedRequest.ContainsKey("img") && decodedRequest.ContainsKey("filter"))
            {
                byte[] hexImg = new byte[0];
                try
                {
                    hexImg = Convert.FromBase64String(decodedRequest["img"].Substring(decodedRequest["img"].IndexOf(",") + 1));
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                    socket.Send("error");
                    return;
                }
                string prefix = decodedRequest["img"].Substring(0, decodedRequest["img"].IndexOf(",") + 1);


               
                bool Runnable = true;
                
                double progress = 0.0;
                        
                Thread progressSend = new Thread(() =>
                {
                    while (Runnable)
                    {
                        socket.Send(string.Format("{0:N2}", progress));
                        Thread.Sleep(100);
                    }
                });
                progressSend.Start();
                Bitmap done;

                if (decodedRequest["filter"] == "1")
                    done = Filters.Filter1((Bitmap)Image.FromStream(new MemoryStream(hexImg)), ref progress);
                else if (decodedRequest["filter"] == "2")
                    done = Filters.Filter2((Bitmap)Image.FromStream(new MemoryStream(hexImg)), ref progress);
                else
                    done = Filters.Filter3((Bitmap)Image.FromStream(new MemoryStream(hexImg)), ref progress);

                Runnable = false;
                progressSend.Join();

                var stream = new MemoryStream();
                done.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                string response = jsonConvert.Serialize(new Dictionary<string, string>() { { "img", prefix + Convert.ToBase64String(stream.ToArray()) } });

                socket.Send(response);
                
            }
            else
            {
                socket.Send("error");
            }
           
        }
        
        static void Main(string[] args)
        {

            // Init WebSocket server
            var server = new WebSocketServer("ws://0.0.0.0:8089");

            server.RestartAfterListenError = true;
            

            // Start WebSocket server
            server.Start(socket =>
            {
                socket.OnMessage = message => processingImg(socket, message);

                socket.OnClose = (() =>
                {
                    socket.Close();
                });
            });

            // Init web server
            int maxThreadsNum = Environment.ProcessorCount * 4;

            ThreadPool.SetMaxThreads(maxThreadsNum, maxThreadsNum);

            ThreadPool.SetMinThreads(2, 2);

            // Start web server
            new Server(8080);

        }
    }
}
