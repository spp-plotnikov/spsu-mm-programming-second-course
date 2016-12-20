using FormsThird;
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
    static class Second
    {
        private static int clientsNum = 4;
        private static List<List<long>> results = new List<List<long>>();

        [STAThread]
        static void Main()
        {
            for (int x = 1; x < 10; x++)
            {
                var svcHost = new ServiceHost(typeof(Service));
                svcHost.Open();
                List<Task<long>> tasks = new List<Task<long>>();
                for (int i = 0; i < clientsNum; i++)
                {
                    tasks.Add(Task.Run(() => Test(x)));
                }
                Task.WaitAll(tasks.ToArray());
                var result = new List<long>();
                foreach (var task in tasks)
                {
                    result.Add(task.Result);
                }
                results.Add(result);
                svcHost.Close();
            }
            Application.Run(new MyForm(results));
        }

        static long Test(int x)
        {
            var counter = System.Diagnostics.Stopwatch.StartNew();
            var callback = new MyCallback();
            var ctx = new InstanceContext(callback);
            var fabric = new DuplexChannelFactory<IService>(ctx, "WSDualHttpBinding_INotificationServices");
            var serv = fabric.CreateChannel();
            byte[] array = new byte[x * x * 256];
            for(int i = 0; i < array.Length; i ++)
            {
                array[i] = 1;
            }
            Task task = Task.Run(() => serv.SendFile("Blur", array));
            Task.WaitAll(task);
            counter.Stop();
            return counter.ElapsedMilliseconds;
        }
    }
}
