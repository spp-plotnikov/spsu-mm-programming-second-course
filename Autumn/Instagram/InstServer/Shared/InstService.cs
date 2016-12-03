using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using BmpLibrary;

namespace Shared
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class InstService : IInstService
    {
        private IInstServiceCallback _notificator;
        public string[] Filters { get; set; } = { "Grey" };

        public event EventHandler OnClientRequestedProcess;
        public event EventHandler OnClientRequestedFilterList;
        private int _currentProgress;
        private Timer _timer;

        public string[] GetFilters()
        {
            OnClientRequestedFilterList?.Invoke(this, null);
            return Filters;
        }

        public byte[] EditPict(byte[] data, string filter)
        {
            OnClientRequestedProcess?.Invoke(this, null);


            _notificator = OperationContext.Current.GetCallbackChannel<IInstServiceCallback>();
            OnProgressChanged(null, new ProgressEventArgs(10));

            byte[] editResult = null;

            /*var thread = new Thread(() =>
            {
                
            });

            thread.Start();*/

            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                var currentBmp = new Bmp(data, OnProgressChanged);


                _timer = new Timer(e =>
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    try
                    {
                        _notificator?.Notify(_currentProgress);
                    }
                    catch (Exception)
                    {
                        //ign
                    }
                }, null, 0, 100);



                typeof(Filters).GetMethod(filter).Invoke(null, new object[] { currentBmp });

                var result = currentBmp.GetResult();

                OnProgressChanged(null, new ProgressEventArgs(85));


                editResult = result;
            }
            catch (Exception)
            {
                OnProgressChanged(null, new ProgressEventArgs(99));

                editResult = null;
            }

            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            return editResult;
        }

        public void OnProgressChanged(object sender, ProgressEventArgs args)
        {
            _currentProgress = args.Progress;
        }
    }
}
