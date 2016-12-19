using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CallbackFunctions : MyServiceReference.IServiceCallback
    {
        public float Progress;
        public byte[] Result;
        public bool IsHere = false;

        public void SendProgress(float progr)
        {
            Progress = progr;            
        }

        public void SendResult(byte[] res)
        {
            Result = res;
            IsHere = true;
        }
    }
}