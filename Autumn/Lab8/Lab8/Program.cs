using System;
using System.ServiceModel;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Server.Service));
            host.Open();
            Console.WriteLine("Host started...");
            Console.WriteLine("Press any button to finish");
            Console.ReadKey();
            host.Close();
        }
    }
}
