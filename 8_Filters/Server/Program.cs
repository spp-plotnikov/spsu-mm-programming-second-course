using System;
using System.Linq;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var svcHost = new ServiceHost(typeof(Service));
            svcHost.Open();
            Console.ReadLine();
            svcHost.Close();
        }
    }
}
