﻿using System.ServiceModel;

namespace Shared
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInstService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IInstServiceCallback))]
    public interface IInstService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract]
        void EditPict(byte[] data, string filter);

    }
}