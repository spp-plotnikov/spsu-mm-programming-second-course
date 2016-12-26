using System;
using System.Windows.Forms;
using Client.ServiceReference;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var client = new ClientConnection();
            Application.Run(new MyClient(client));
        }
    }
}
