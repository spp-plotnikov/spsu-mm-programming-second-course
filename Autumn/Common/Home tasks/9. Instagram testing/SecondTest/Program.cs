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
using System.Diagnostics;
using System.Windows.Forms;

namespace SecondTest
{
    static class Program
    {
        private static string localhost = "net.tcp://localhost:8080/";
        private static int bufSize = 10 * 1024 * 1024; // 10 MB
        private static readonly int numTry = 5;
        private static readonly int numImages = 5;
        private static List<int> medTime = new List<int>();
        private static List<int> maxTime = new List<int>();
        private static List<int> midTime = new List<int>();
        private static List<string> sizes = new List<string>(); // string для маштабирования, иначе график плохо смотрится

        static int Emulate(Bitmap image)
        {
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
            for (int i = 1; i <= numImages; i++)
            {
                string path =  i + ".bmp";
                Bitmap image = new Bitmap(path);

                Console.WriteLine("image: " + path);

                List<int> result = new List<int>();
                for (int j = 0; j < numTry; j++)
                {
                    int cur = Emulate(image);
                    result.Add(cur);
                    Console.WriteLine("Time: " + cur);
                }
                result.Sort();

                maxTime.Add(result[numTry - 1]);
                medTime.Add(result[numTry / 2]);
                midTime.Add(result.Sum() / result.Count);

                int size = image.Width * image.Height;
                Console.WriteLine("Size: " + size + ", Max: " + result[numTry - 1] + ", Medium time: " + result[numTry / 2] + ", Average time: " + result.Sum() / result.Count);
                sizes.Add(size.ToString());
            }
            Application.Run(new Form1(midTime, medTime, maxTime, sizes));

            Console.ReadLine();
        }
    }
}