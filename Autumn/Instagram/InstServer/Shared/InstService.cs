using System;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using BmpLibrary;

namespace Shared
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class InstService : IInstService
    {

        public event EventHandler OnClientRequestedProcess;
        public event EventHandler OnClientRequestedFilterList;
        private Timer _timer;
        private IInstServiceCallback _notificator;

        public string[] GetFilters()
        {
            OnClientRequestedFilterList?.Invoke(this, null);
            string[] result = null;
            try
            {
                StreamReader sr = File.OpenText(Path.GetFullPath("filters.config"));
                string filters = sr.ReadToEnd();
                result = filters.Split(' ');
            }
            catch
            {
                //ign
            }
            return result;
        }

        public byte[] EditPict(byte[] data, string filter)
        {
            _notificator = OperationContext.Current.GetCallbackChannel<IInstServiceCallback>();
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
                        _notificator?.Notify(curProgress);
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

            return editResult;
        }
    }
}
