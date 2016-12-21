using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerTestSystem
{
    class Client
    {
        MyServiceReference.ServiceClient client;
        MyCallback callback;
        Bitmap imageBitmap;
        int number;
        public Client(int i)
        {
            number = i;
            callback = new MyCallback();
            var instanceContext = new InstanceContext(callback);
            client = new MyServiceReference.ServiceClient(instanceContext, "WSDualHttpBinding_IService");
        }
      
        public float SendPicture(string file)
        {
            StreamReader picture = new StreamReader(@file);
            imageBitmap = new Bitmap(picture.BaseStream);
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                client.GetPicture(imageBitmap, "Grey");
                while (!callback.IsHere)
                {
                    Console.WriteLine("#{0} - {1} progress {2}", number, callback.Progress, client.State);
                    System.Threading.Thread.Sleep(100);
                }
                stopWatch.Stop();
            }
            catch
            {
                return -1;
            }

            Console.WriteLine("time:" + (stopWatch.ElapsedMilliseconds / 1000.0f));


            return stopWatch.ElapsedMilliseconds / 1000.0f;
        }     

    }
}
