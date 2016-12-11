﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Filters
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        long SizeOfResult
        {
            [OperationContract]
            get;
        }

        [OperationContract]
        List<string> Filters();

        [OperationContract]
        int GetProgress();
        
        [OperationContract]
        void SetFilter(string filterName);

        [OperationContract]
        void DoFilter();

        [OperationContract]
        bool SendChunk(byte[] chunk, long fstBytePosition, long sizeOfChunk);

        [OperationContract]
        void SetByteArray(long arrayLength);

        [OperationContract]
        byte[] GetChunk();

        [OperationContract]
        Bitmap GetResultBitmap();
        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "Filters.ContractType".
  
}
