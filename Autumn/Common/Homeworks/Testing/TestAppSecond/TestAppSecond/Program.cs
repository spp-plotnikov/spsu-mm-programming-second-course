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

namespace TestAppSecond
{
    static class Program
    {
        static private List<Bitmap> _images = new List<Bitmap>();
        [STAThread]
        static void Main()
        {
            SetImages();
            List<long> medians = new List<long>();
            List<long> maxValues = new List<long>();
            List<long> mid = new List<long>();
            int concurrentClients = 3;
            for (int sizeOfImage = 0; sizeOfImage < 4; sizeOfImage++)
            {
                List<Task<long>> workingClients = new List<Task<long>>();
                Bitmap toSend = _images.ElementAt(sizeOfImage);
                for (int i = 0; i < concurrentClients; i++)
                {
                    workingClients.Add(Task.Run(() => Test(sizeOfImage, toSend)));
                }
                Task.WaitAll(workingClients.ToArray());
                var result = new List<long>();
                foreach (var work in workingClients)
                {
                    result.Add(work.Result);
                }
                result.Sort();
                maxValues.Add(result[concurrentClients - 1]);
                medians.Add(result[concurrentClients / 2]);
                mid.Add(result.Sum() / result.Count());
            }
            List<String> imagesSize = new List<string>();
            foreach(var image in _images)
            {
                int size = image.Width * image.Height;
                imagesSize.Add(size.ToString());
            }
            Application.Run(new TestSecondForm(mid, medians, maxValues, imagesSize));
        }

        static void SetImages()
        {
            Bitmap one = TestAppSecond.Properties.Resources._1;
            Bitmap two = TestAppSecond.Properties.Resources._2;
            Bitmap three = TestAppSecond.Properties.Resources._3;
            Bitmap four = TestAppSecond.Properties.Resources._4;
            _images.Add(one);
            _images.Add(two);
            _images.Add(three);
            _images.Add(four);
        }

        static long Test(int size, Bitmap nimage)
        {
            var counter = System.Diagnostics.Stopwatch.StartNew();
            Bitmap image = (Bitmap)nimage.Clone();
            int bufferSize = 2147483647;
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferPoolSize = bufferSize;
            binding.MaxBufferSize = bufferSize;
            binding.MaxReceivedMessageSize = bufferSize;
            ChannelFactory<IService> cf = new ChannelFactory<IService>(binding, "net.tcp://127.0.0.1:11000/");
            IService filter = cf.CreateChannel();
            Thread threadFilter = new Thread(() => { filter.Filter(image, 1); });
            threadFilter.IsBackground = true;
            threadFilter.Start();
            int progress = 0;
            while (progress != 100)
            {
                progress = filter.GetProgress();
            }
            byte[] newImage = filter.GetImage();
            counter.Stop();
            return counter.ElapsedMilliseconds;
        }
    }
}
