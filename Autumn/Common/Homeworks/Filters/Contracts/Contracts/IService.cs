using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Server
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract]
        void CancelProgress();

        [OperationContract]
        bool CheckIsAlive();

        [OperationContract]
        void ChangeIsAlive(bool x);

        [OperationContract]
        int GetProgress();

        [OperationContract]
        byte[] GetImage();

        [OperationContract]
        byte[] Filter(Bitmap image, int index);
    }
}
