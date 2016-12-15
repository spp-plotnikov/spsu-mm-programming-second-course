using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(ICallbackService))]
    public interface IService
    {
        [OperationContract]
        List<string> GetFilters();

        [OperationContract(IsOneWay = true)]
        void GetPicture(Bitmap map, string filter);

        [OperationContract(IsOneWay = true)]
        void StopWork();
    }
}
