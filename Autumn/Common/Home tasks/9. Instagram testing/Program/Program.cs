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

namespace Program
{
    static class Program
    {
        private static string localhost = "net.tcp://localhost:8080/";
        private static int bufSize = 10 * 1024 * 1024; // 10 MB

        [STAThread]
        static void Main()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferSize = bufSize;
            binding.MaxBufferPoolSize = bufSize;
            binding.MaxReceivedMessageSize = bufSize;
            ChannelFactory<IService> cf = new ChannelFactory<IService>(binding, localhost);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form f1 = new Form1(cf.CreateChannel());
            Application.Run(f1);
            
        }
    }
}

