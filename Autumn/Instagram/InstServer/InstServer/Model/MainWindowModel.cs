using System;
using System.ServiceModel;
using System.Threading;
using Shared;

namespace InstServer.Model
{
    public class MainWindowModel
    {
        private InstService _service;
        private bool _isServiceEnabled;

        public event ModelEventHandler SendInformToConsole;
        public event EventHandler ServerStatusChanged;
        public string[] CurrentFilters { get; set; }

        public void OpenService(object port)
        {

            _service = new InstService();
            _service.OnClientRequestedFilterList += FiltersRequestHandler;
            _service.OnClientRequestedProcess += ProcessRequestHandler;
            _service.Filters = CurrentFilters;

            Thread thread = new Thread(() =>
            {
                using (var host = new ServiceHost(_service))
                {
                    _isServiceEnabled = true;

                    host.Open();
                    ServerStatusChanged?.Invoke(this, null);
                    SendInformToConsole?.Invoke(this, new ModelEventArgs("Server has been successfully launched..."));

                    while (_isServiceEnabled)
                    {
                        
                    }

                    ServerStatusChanged?.Invoke(this, null);
                    SendInformToConsole?.Invoke(this, new ModelEventArgs("Server has been successfully shut down..."));
                }
            });
            thread.Start();
        }

        public void CloseService(object obj)
        {
            _isServiceEnabled = false;
        }

        public void FiltersRequestHandler(object sender, EventArgs args)
        {
            SendInformToConsole?.Invoke(this, new ModelEventArgs("Requested filters..."));
        }

        public void ProcessRequestHandler(object sender, EventArgs args)
        {
            SendInformToConsole?.Invoke(this, new ModelEventArgs("Requested pict process..."));
        }

        public void UpdateFilterList(string[] filters)
        {
            CurrentFilters = filters;
            _service.Filters = filters;
        }
    }
}