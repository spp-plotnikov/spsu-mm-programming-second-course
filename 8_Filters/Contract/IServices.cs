using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

namespace Server
{
    [ServiceContract(SessionMode=SessionMode.Required, CallbackContract = typeof(IServiceCallBack))]
    public interface IService
    {
        [OperationContract]
        List<string> ReadFilters();
        [OperationContract(IsOneWay = true)]
        void SendFile(string filterName, Bitmap bytes);
        [OperationContract(IsOneWay = true)]
        void SendProgress();
        [OperationContract(IsOneWay = true)]
        void Cancel();
    }

    public interface IServiceCallBack
    {
      
        [OperationContract]
        void GetImage(Bitmap image);
        [OperationContract]
        void GetProgress(int progress);

    }

}
