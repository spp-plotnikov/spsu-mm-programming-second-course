using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        List<string> GetFilters(string fileName);
    }
}
