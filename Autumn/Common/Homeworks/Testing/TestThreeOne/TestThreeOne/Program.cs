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

namespace TestThreeOne
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static bool Working = true;
        [STAThread]
        static void Main()
        {
            int concurrentClients = 1;
            List<long> time = new List<long>();
            List<string> people = new List<string>();
            while (Working)
            {
                try
                {
                    var counter = System.Diagnostics.Stopwatch.StartNew();
                    List<Task<long>> workingClients = new List<Task<long>>();
                    for (int i = 0; i < concurrentClients; i++)
                    {
                        workingClients.Add(Task.Run(() => Test()));
                    }
                    Task.WaitAll(workingClients.ToArray());
                    counter.Stop();
                    time.Add(counter.ElapsedMilliseconds);
                    people.Add(concurrentClients.ToString());
                    concurrentClients++;     
                }
                catch(Exception)
                {
                    Working = false;
                }
            }
            Application.Run(new TestThreeOne(time, people));
        }

        static long Test()
        {
            var counter = System.Diagnostics.Stopwatch.StartNew();
            Bitmap image = new Bitmap(1024, 1024);
            int bufferSize = 2147483647;
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferPoolSize = bufferSize;
            binding.MaxBufferSize = bufferSize;
            binding.MaxReceivedMessageSize = bufferSize;
            ChannelFactory<IService> cf = new ChannelFactory<IService>(binding, "net.tcp://127.0.0.1:11000/");
            IService filter = cf.CreateChannel();
            Thread threadFilter = new Thread(() =>
            {
                try
                {
                    filter.Filter(image, 1);
                }
                catch(Exception)
                {
                    Working = false;
                }
            });
            threadFilter.IsBackground = true;
            threadFilter.Start();
            int progress = 0;
            while (progress != 100 && Working)
            {
                try
                {
                    progress = filter.GetProgress();
                }
                catch(Exception)
                {
                    Working = false;
                }
            }
            try
            {
                byte[] newImage = filter.GetImage();
            }
            catch(Exception)
            {
                Working = false;
            }
            counter.Stop();
            return counter.ElapsedMilliseconds;
        }
    }
}
