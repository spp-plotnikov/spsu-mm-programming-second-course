using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            MyService server = new MyService();
            ServiceHost host = new ServiceHost(server);
            host.AddServiceEndpoint(typeof(IMyService), 
                new BasicHttpBinding(), "http://localhost:81");
            host.Open();
            Console.WriteLine("Server started");
            Console.ReadKey();
            host.Close();
        }
    }
}
