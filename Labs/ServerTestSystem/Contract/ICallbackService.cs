using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface ICallbackService
    {
        [OperationContract(IsOneWay = true)]
        void SendResult(byte[] res);

        [OperationContract(IsOneWay = true)]
        void SendProgress(float progr);
    }
}
