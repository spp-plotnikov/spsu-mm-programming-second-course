using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Server
{
    class Program
    {
        private static string localhost = "net.tcp://localhost:8080/";
        private static int bufSize = 10 * 1024 * 1024;

        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service), new Uri(localhost));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferSize = bufSize;
            binding.MaxBufferPoolSize = bufSize;
            binding.MaxReceivedMessageSize = bufSize;
            host.AddServiceEndpoint(typeof(IService), binding, localhost);
            host.Open();
            Console.WriteLine("Open");
            Console.ReadKey();
            Console.WriteLine("Close");
            host.Close();
        }
    }
}
