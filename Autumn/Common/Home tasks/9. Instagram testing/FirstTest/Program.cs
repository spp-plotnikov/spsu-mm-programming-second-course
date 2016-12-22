using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Drawing;
using System.ServiceModel.Description;
using Server;
using System.Windows.Forms;
using System.Diagnostics;

namespace FirstTest
{
    static class Program
    {
        private static string localhost = "net.tcp://localhost:8080/";
        private static int bufSize = 10 * 1024 * 1024; // 10 MB
        private static List<int> numPeople = new List<int>();
        private static List<int> medTime = new List<int>();
        private static List<int> maxTime = new List<int>();
        private static List<int> midTime = new List<int>();

        static int Emulate()
        {
            Bitmap image = new Bitmap(720, 480); // set image 720 * 480

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferSize = bufSize;
            binding.MaxBufferPoolSize = bufSize;
            binding.MaxReceivedMessageSize = bufSize;
            ChannelFactory<IService> cf = new ChannelFactory<IService>(binding, localhost);
            IService filter = cf.CreateChannel();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Thread filtering = new Thread(() => { filter.Filter(image, 1); });
            filtering.IsBackground = true;
            filtering.Start();

            // emulate 
            int progress = 0;
            while (progress != 100)
            {
                progress = filter.GetProgress();
            }

            // get result
            byte[] newImage = filter.GetImage();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            return (int)ts.TotalMilliseconds;
        }

        [STAThread]
        static void Main()
        {
            int numClients = 1;
            while (true)
            {
                List<Task<int>> workingClients = new List<Task<int>>();
                for (int i = 0; i < numClients; i++)
                {
                    workingClients.Add(Task.Run(() => Emulate()));
                }
                Task.WaitAll(workingClients.ToArray());


                List<int> result = new List<int>();
                for (int i = 0; i < numClients; i++)
                {
                    result.Add(workingClients[i].Result);
                }
                result.Sort();

                int max = result[numClients - 1];
                maxTime.Add(max);
                medTime.Add(result[numClients / 2]);
                midTime.Add(result.Sum() / result.Count);
                    
                Console.WriteLine("Clients: " + numClients + ", Max time: " + max);
                numPeople.Add(numClients);
                numClients++;

                // Stop == (timeout > 10000 mls)
                if (max > 10000 || numClients > 53)
                {
                    if (numClients == 54)
                    {
                        maxTime[maxTime.Count - 1] = 11000;
                    }
                    break;
                }
            }
            Application.Run(new Form1(midTime, medTime, maxTime, numPeople));
        }
    }
}