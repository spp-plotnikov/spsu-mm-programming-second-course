using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface IInstServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Notify(int progress);
    }
}