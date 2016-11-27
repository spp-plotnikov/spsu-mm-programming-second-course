using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Shared
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInstService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IInstServiceCallback))]
    public interface IInstService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract]
        byte[] EditPict(byte[] data, string filter);
    }
}
