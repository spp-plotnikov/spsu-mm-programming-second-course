using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contract
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        List<string> filt = new List<string>();
        Filters current;
        byte[] result;
        Task<byte[]> task = null;
        ICallbackService callback;
        bool finish = false;
        public Service()
        {
            callback = OperationContext.Current.GetCallbackChannel<ICallbackService>();

            StreamReader file = new StreamReader(@"..\..\..\Host\FilterList.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                filt.Add(line);
                //Console.WriteLine(line);
            }
        }

        public List<string> GetFilters()
        {
            return filt;
        }

        public void ProgressPerSecond()
        {
            Console.WriteLine("Start PerSec");
            float progress = current.Progress();
            while(progress < 100.0 && current.Stop == false)
            {
                if (finish) return;
                callback.SendProgress(progress);
                Console.WriteLine(progress);
                Thread.Sleep(1000);
                progress = current.Progress();
            }
            result = task.Result;
            Console.WriteLine("Result");
            callback.SendProgress(100);
            callback.SendResult(result);
        }

        public void GetPicture(Bitmap map, string filter)
        {
            current = new Filters(map);
            Console.WriteLine("GetPict");
            using (MemoryStream memStream = new MemoryStream())
            {
                switch (filter)//task!!!!
                {
                    case "Grey":
                        task = Task.Run(() => current.GreyFilter());
                        break;
                    case "Inversion":
                        task = Task.Run(() => current.InvertFilter());
                        break;
                    case "Sepia":
                        task = Task.Run(() => current.SepiaFilter());
                        break;
                    default:
                        result = null;
                        break;
                }
                Console.WriteLine("Exit");
            }
            float progress = current.Progress();
            while (progress < 100.0 && current.Stop == false)
            {
                if (finish) return;
                callback.SendProgress(progress);
                Console.WriteLine(progress);
                Thread.Sleep(1000);
                progress = current.Progress();
            }
            result = task.Result;
            Console.WriteLine("Result");
            callback.SendProgress(100);
            callback.SendResult(result);
        }

        public void StopWork()
        {
            Console.WriteLine("Stop pressed");
            current.Stop = true;           
        }
    }
}
