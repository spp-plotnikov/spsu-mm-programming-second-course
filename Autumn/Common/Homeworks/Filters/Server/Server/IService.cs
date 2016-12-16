using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace Server
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract]
        void SetProgress();

        [OperationContract]
        bool CheckIsAlive();

        [OperationContract]
        void ChangeIsAlive(bool x);

        [OperationContract]
        int GetProgress();

        [OperationContract]
        byte[] GetImage();

        [OperationContract]
        void Filter(Bitmap image, int index);
    }
}
