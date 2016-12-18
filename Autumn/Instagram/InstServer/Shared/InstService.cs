using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using BmpLibrary;

namespace Shared
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class InstService : IInstService
    {
        public string[] Filters { get; set; } = { "Grey" };

        public event EventHandler OnClientRequestedProcess;
        public event EventHandler OnClientRequestedFilterList;
        private Timer _timer;

        public string[] GetFilters()
        {
            OnClientRequestedFilterList?.Invoke(this, null);
            return Filters;
        }

        //private void Notify()

        public void EditPict(byte[] data, string filter)
        {
            var notificator = OperationContext.Current.GetCallbackChannel<IInstServiceCallback>();
            Task.Run(() => Edit(data, filter, notificator));
        }

        private void Edit(byte[] data, string filter, IInstServiceCallback notificator)
        {
            OnClientRequestedProcess?.Invoke(this, null);

            object curProgress = 10;

            byte[] editResult;

            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                var currentBmp = new Bmp(data, ((sender, args) => { curProgress = args.Progress; }));


                _timer = new Timer(e =>
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    try
                    {
                        notificator?.Notify(curProgress);
                    }
                    catch (Exception)
                    {
                        //ign
                    }
                }, null, 0, 100);



                typeof(Filters).GetMethod(filter).Invoke(null, new object[] { currentBmp });

                var result = currentBmp.GetResult();

                curProgress = 85;


                editResult = result;
            }
            catch (Exception)
            {
                curProgress = 99;

                editResult = null;
            }

            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            notificator.GetPict(editResult);
        }
    }
}
