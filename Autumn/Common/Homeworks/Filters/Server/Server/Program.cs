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
            host.AddDefaultEndpoints();

            host.Open();
            Console.WriteLine("Сервис открыт");

            Console.ReadKey();
            host.Close();
        }
    }
}
