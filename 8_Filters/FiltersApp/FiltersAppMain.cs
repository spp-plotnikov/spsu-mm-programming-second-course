﻿using System;
using System.Windows.Forms;
using System.ServiceModel;
using Server;

namespace Forms
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var myCallback = new MyCallback();
            var ctx = new InstanceContext(myCallback);
            var fabric = new DuplexChannelFactory<IService>(ctx, "WSDualHttpBinding_INotificationServices");
            Application.Run(new Forms.FiltersApp(fabric.CreateChannel(), myCallback));
        }

    }
}
