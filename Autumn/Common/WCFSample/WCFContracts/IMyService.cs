using System.ServiceModel;

namespace WCFContracts
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string SayHello(string name);
    }
}
