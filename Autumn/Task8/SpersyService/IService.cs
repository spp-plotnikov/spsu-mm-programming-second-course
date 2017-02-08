using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SpersyService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IService
    {
        [OperationContract]
        string[] GetFilterNames();
        [OperationContract]
        void ProceedImage(byte[] bytes, string filterType);
        [OperationContract]
        void CancelProceeding();
    }

    [ServiceContract]
    public interface IServiceCallback
    {
        [OperationContract]
        void GetProgress(int percents);

        [OperationContract]
        void GetImage(byte[] image);
    }
}
