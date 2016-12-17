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
        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service), new Uri("net.tcp://localhost:11000/"));
            //host.AddDefaultEndpoints();
            WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647; 
            host.AddServiceEndpoint(typeof(IService), binding, "http://localhost:11000/");
            ServiceDebugBehavior stp = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            host.Open();
            Console.WriteLine("Сервис открыт");

            Console.ReadKey();
            host.Close();
        }
    }
}
