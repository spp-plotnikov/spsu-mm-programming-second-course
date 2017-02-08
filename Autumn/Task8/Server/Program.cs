using SpersyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(SpersyService.SpersyService));
            host.AddServiceEndpoint(typeof(IService), new NetTcpBinding(), "net.tcp://localhost:8080");
            host.Open();
            Console.ReadLine();
            host.Close();
        }
    }
}
