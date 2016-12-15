using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service : IService
    {
        List<string> filt = new List<string>();
        Filters current;
        byte[] result;
        ICallbackService callback;
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

        private void ProgressPerSecond()
        {
            float progress = current.Progress();
            while(progress < 100.0)
            {
                callback.SendProgress(progress);
                Console.WriteLine(progress);
                System.Threading.Thread.Sleep(1000);
                progress = current.Progress();
            }

        }
        public void GetPicture(Bitmap map, string filter)
        {
            current = new Filters(map);
            Console.WriteLine("GetPict");
            Task<byte[]> task = null;
            switch (filter)//task!!!!
            {
                case "Grey":
                    task = Task.Run(() => current.GreyFilter());
                    ProgressPerSecond();
                    result = task.Result;
                    break;
                case "Inversion":
                    task = Task.Run(() => current.InvertFilter());
                    ProgressPerSecond();
                    result = task.Result;
                    break;
                case "Sepia":
                    task = Task.Run(() => current.SepiaFilter());
                    ProgressPerSecond();
                    result = task.Result;
                    break;
                default:
                    result = null;
                    break;
            }
            Console.WriteLine("Result");
            callback.SendProgress(100);
            callback.SendResult(result);
        }

        public void StopWork()
        {
            current.Stop();
        }
    }
}
