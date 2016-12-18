using System;
using System.IO;
using InstServer.Command;
using InstServer.Model;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace InstServer.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _consoleLog;
        private string _changeServerStatusBtnName;
        private RelayCommand _changeServerStatus;
        private SimpleCommand _getFilters;
        private readonly MainWindowModel _model;
        private bool _isServerEnabled;


        public MainWindowViewModel(MainWindowModel model)
        {
            _model = model;
            model.SendInformToConsole += OnConsoleLogChanged;
            model.ServerStatusChanged += OnServerStatusChanged;

            _changeServerStatusBtnName = "Launch server";
            _getFilters = new SimpleCommand(GetFiltersFromDialogWindow);
            _changeServerStatus = new RelayCommand(model.OpenService);

            try
            {
                StreamReader sr = File.OpenText(Path.GetFullPath("filters.config"));
                string filters = sr.ReadToEnd();
                OnConsoleLogChanged(this, new ModelEventArgs("Readed filters: " + filters));
                _model.CurrentFilters = filters.Split(' ');
            }
            catch (Exception)
            {
                OnConsoleLogChanged(this, new ModelEventArgs("Can't reach filters.config"));
            }

        }



        public string ChangeServerStatusBtnName
        {
            get
            {
                return _changeServerStatusBtnName;
                
            }
            set
            {
                if(Equals(value, _changeServerStatusBtnName)) return;
                _changeServerStatusBtnName = value;
                OnPropertyChanged();
            }
        }  

        public string ConsoleLog
        {
            get
            {
                return _consoleLog;
            }
            set
            {
                if (Equals(_consoleLog, value)) return;
                _consoleLog = value + "\n";
                OnPropertyChanged();
            }
        } 

        public RelayCommand ChangeServerStatus
        {
            get
            {
                return _changeServerStatus;

            }
            set
            {
                if (Equals(value, _changeServerStatus)) return;
                _changeServerStatus = value;
                OnPropertyChanged();
            }
        }

        public SimpleCommand GetFilters
        {
            get
            {
                return _getFilters;
                
            }
            set
            {
                if(Equals(value, _getFilters)) return;
                _getFilters = value;
            }
        }


        public void GetFiltersFromDialogWindow()
        {
            if (!_isServerEnabled)
            {
                OnConsoleLogChanged(this, new ModelEventArgs("Cannot update filter list: server is disabled"));
                return;
            }
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Config|*.config",
                CheckFileExists = true
            };
            if (dlg.ShowDialog() == true)
            {
                StreamReader sr = File.OpenText(dlg.FileName);
                string filters = sr.ReadToEnd();
                OnConsoleLogChanged(this, new ModelEventArgs("Readed filters: " + filters));
                _model.UpdateFilterList(filters.Split(' '));
            }
        }

        public void InitiateFiltersList(string path)
        {
            
        }


        public void OnServerStatusChanged(object sender, EventArgs args)
        {
            if (ChangeServerStatusBtnName == "Launch server")
            {
                _isServerEnabled = true;
                ChangeServerStatusBtnName = "Shutdown server";
                ChangeServerStatus = new RelayCommand(_model.CloseService);
            }
            else
            {
                _isServerEnabled = false;
                ChangeServerStatusBtnName = "Launch server";
                ChangeServerStatus = new RelayCommand(_model.OpenService);
            }
        }

        public void OnConsoleLogChanged(object sender, ModelEventArgs args)
        {
            if (args.ServerInformation != null)
            {
                ConsoleLog += args.ServerInformation;
            }
        }

        public void OnWindowClosing(object sender, EventArgs args)
        {
            if(_isServerEnabled)
                _model.CloseService(null);
        }

    }
}