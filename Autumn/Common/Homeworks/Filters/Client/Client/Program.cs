using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Windows.Forms;
using Server;

namespace Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
           // var cf = new ChannelFactory<IService>(new NetTcpBinding(), "net.tcp://127.0.0.1:11000/");
            WebHttpBinding binding = new WebHttpBinding();
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            ChannelFactory<IService> cf = new ChannelFactory<IService>(binding, "http://localhost:11000/");
            cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormFilters(cf.CreateChannel()));
        }
    }
}
