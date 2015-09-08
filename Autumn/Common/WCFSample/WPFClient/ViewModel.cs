using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WCFContracts;
using WPFClient.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace WPFClient
{
    class ViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name { get; set; }

        public string Result { get; set; }

        public ICommand RequestData
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        try
                        {
                            var cf = new ChannelFactory<IMyService>(new NetTcpBinding(SecurityMode.None),
                                        "net.tcp://localhost:456/service");

                            var client = cf.CreateChannel();

                            Result = client.SayHello(Name);
                        }
                        catch (Exception)
                        {
                        }


                        OnPropertyChanged("Result");
                    });
            }
            
        }
    }
}
