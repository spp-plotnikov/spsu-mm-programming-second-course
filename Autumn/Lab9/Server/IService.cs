using System.ServiceModel;

namespace Server
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        byte[] SendImage(byte[] image, string filter);
    }
}
