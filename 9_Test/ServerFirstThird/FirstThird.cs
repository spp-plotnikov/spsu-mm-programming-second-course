using Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    static class FirstThird
    {
        private static bool flag = true;
        private static int clientsNum = 1;
        private static List<List<long>> results = new List<List<long>>();

        [STAThread]
        static void Main()
        {
            while (flag)
            {
                try
                {
                    var svcHost = new ServiceHost(typeof(Service));
                    svcHost.Open();
                    List<Task<long>> tasks = new List<Task<long>>();
                    for (int i = 0; i < clientsNum; i++)
                    {
                        tasks.Add(Task.Run(() => Test()));
                    }
                    Task.WaitAll(tasks.ToArray());
                    var result = new List<long>();
                    foreach (var task in tasks)
                    {
                        result.Add(task.Result);
                    }
                    results.Add(result);
                    svcHost.Close();
                    clientsNum += 1;
                }
                catch(Exception)
                {
                    flag = false;
                }
            }
            Application.Run(new MyForm(results));
        }

        private static long Test()
        {
            var counter = System.Diagnostics.Stopwatch.StartNew();
            var callback = new MyCallback();
            var ctx = new InstanceContext(callback);
            var fabric = new DuplexChannelFactory<IService>(ctx, "WSDualHttpBinding_INotificationServices");
            var serv = fabric.CreateChannel();
            using (FileStream fstream = File.OpenRead("test.png"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                Task task = Task.Run(() => serv.SendFile("Blur", array));
                Task.WaitAll(task);
            }
            counter.Stop();
            return counter.ElapsedMilliseconds;
        }
    }
}
