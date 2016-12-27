using System.ServiceModel;

namespace Server
{
    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract]
        void SendImage(byte[] image, string filter);

        [OperationContract]
        void SendCancel(bool cancel);
    }

    [ServiceContract]
    public interface IServiceCallback
    {
        [OperationContract]
        void GetProgress(int progress);

        [OperationContract]
        void GetImage(byte[] image);
    }
}
