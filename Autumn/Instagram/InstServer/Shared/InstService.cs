using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BmpLibrary;

namespace Shared
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InstService : IInstService
    {
        private IInstServiceCallback _notificator;
        public string[] Filters { get; set; } = { "Grey" };

        public event EventHandler OnClientRequestedProcess;
        public event EventHandler OnClientRequestedFilterList;

        public string[] GetFilters()
        {
            OnClientRequestedFilterList?.Invoke(this, null);
            return Filters;
        }

        public byte[] EditPict(byte[] data, string filter)
        {
            try
            {
                Bmp currentBmp = new Bmp(data, NotifyClient);
                OnClientRequestedProcess?.Invoke(this, null);

                typeof(Filters).GetMethod(filter).Invoke(null, new object[] { currentBmp });

                var result = currentBmp.GetResult();

                NotifyClient(null, new ProgressEventArgs(85));

                return result;
            }
            catch (Exception)
            {
                NotifyClient(null, new ProgressEventArgs(99));
                //ing
            }

            return null;
        }

        public void NotifyClient(object sender, ProgressEventArgs args)
        {
            _notificator = OperationContext.Current.GetCallbackChannel<IInstServiceCallback>();
            _notificator.Notify(args.Progress);
        }
    }
}
