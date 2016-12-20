using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

namespace Server
{
    [ServiceContract(SessionMode=SessionMode.Required, CallbackContract = typeof(IServiceCallBack))]
    public interface IService
    {
       
        [OperationContract(IsOneWay = true)]
        void SendFile(string filterName, byte[] bytes);
        [OperationContract(IsOneWay = true)]
        void Cancel();
    }

    public interface IServiceCallBack
    {
        [OperationContract]
        void GetImage(byte[] bytes, bool flag);
        [OperationContract]
        void GetProgress(int progress);

    }

}
