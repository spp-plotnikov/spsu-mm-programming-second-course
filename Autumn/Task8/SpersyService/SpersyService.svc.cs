using FiltersLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpersyService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpersyService : IService
    {
        private bool isCancelled;
        private IServiceCallback client;
        private Filters currentFilter;

        public SpersyService()
        {
            client = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            currentFilter = new Filters();
            isCancelled = false;
        }
        public void CancelProceeding()
        {
            isCancelled = true;
        }

        public string[] GetFilterNames()
        {
            return new Filters().GetFilters();
        }

        public void ProceedImage(byte[] bytes, string filterType)
        {
            Bitmap image = (Bitmap)Image.FromStream(new MemoryStream(bytes));
            client.GetProgress(currentFilter.Percents);
            Task<Bitmap> newImage = Task.Run(() => currentFilter.ApplyFilter(image, filterType));

            while (currentFilter.Percents != 95 && !isCancelled)
            {
                if (isCancelled)
                {
                    newImage.Dispose();
                    break;
                }
                Thread.Sleep(100);
                client.GetProgress(currentFilter.Percents);
            }

            if (!isCancelled)
            {
                client.GetImage((byte[])(new ImageConverter().ConvertTo(newImage.Result, typeof(byte[]))));
            }
            else
            {
                client.GetImage(bytes);
            }
            isCancelled = false;
        }
    }
}
