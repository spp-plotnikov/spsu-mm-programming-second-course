using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using WCFContracts;

namespace WCFSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof (IMyService), new Uri("http://localhost:3456/"));

            host.AddServiceEndpoint(typeof (MyService),
                                    new NetTcpContextBinding(SecurityMode.Message)
                                    , "service");

            var metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                host.Description.Behaviors.Add(metadataBehavior = new ServiceMetadataBehavior());
            }

            host.AddServiceEndpoint(typeof (IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            host.Open();
            Console.ReadKey();
            host.Close();
        }
    }
}
