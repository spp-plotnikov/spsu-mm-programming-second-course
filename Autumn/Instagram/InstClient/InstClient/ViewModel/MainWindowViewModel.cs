using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using InstClient.Command;
using InstClient.Model;
using Microsoft.Win32;

namespace InstClient.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private int _processingProgress;
        private CollectionView _filtersCollectionView;
        private string _initalPictPath;
        private string _resultPictPath;
        private bool _isInitalPictVisible;
        private bool _isResultPictVisible;
        private SimpleCommand _updateFilterList;
        private SimpleCommand _openPict;
        private SimpleCommand _sendPict;
        private SimpleCommand _saveResult;
        private ClientModel _model;

        public MainWindowViewModel(ClientModel model)
        {
            _model = model;
            model.ReceivedFilters += OnReceivedFilters;
            model.GotAnyError += OnAnyError;
            model.PictProcessed += OnPictProcessed;
            model.ProgressChanged += OnProgressChanged;
            OpenPict = new SimpleCommand(GetOpenedPictPath);
            UpdateFilterList = new SimpleCommand(model.GetFilters);
            SendPict = new SimpleCommand(UploadPict);
            SaveResult = new SimpleCommand(SaveResultWithDialog);
            IsInitalPictVisible = true;
            IsResultPictVisible = false;
        }

        public SimpleCommand SaveResult
        {
            get
            {
                return _saveResult;
                
            }
            set
            {
                _saveResult = value;
                OnPropertyChanged();
            }
        }

        public bool IsInitalPictVisible
        {
            get
            {
                return _isInitalPictVisible;
                
            }
            set
            {
                _isInitalPictVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsResultPictVisible
        {
            get
            {
                return _isResultPictVisible;

            }
            set
            {
                _isResultPictVisible = value;
                OnPropertyChanged();
            }
        }

        public SimpleCommand SendPict
        {
            get
            {
                return _sendPict;
                
            }
            set
            {
                if(Equals(value, _sendPict)) return;
                _sendPict = value;
                OnPropertyChanged();
            }
        }


        public SimpleCommand OpenPict
        {
            get
            {
                return _openPict;
                
            }
            set
            {
                if(Equals(value, _openPict)) return;
                _openPict = value;
                OnPropertyChanged();
            }
        }

        public SimpleCommand UpdateFilterList
        {
            get
            {
                return _updateFilterList;
            }
            set
            {
                if(Equals(value, _updateFilterList)) return;
                _updateFilterList = value;
                OnPropertyChanged();
            }
        }


        public string ResultPictPath
        {
            get
            {
                return _resultPictPath;
                
            }
            set
            {
                _resultPictPath = value;
                OnPropertyChanged();
            }

        }

        public string InitalPictPath
        {
            get
            {
                return _initalPictPath;
                
            }
            set
            {
                if (Equals(value, _initalPictPath))
                {
                    ResultPictPath = null;
                    return;
                }
                _initalPictPath = value;
                ResultPictPath = null;
                IsInitalPictVisible = true;
                IsResultPictVisible = false;
                OnPropertyChanged();
            }
        }

        public int ProcessingProgress
        {
            get
            {
                return _processingProgress;
                
            }
            set
            {
                if(Equals(_processingProgress, value)) return;
                _processingProgress = value;
                OnPropertyChanged();
            }
        }

        public CollectionView FiltersCollectionView
        {
            get
            {
                return _filtersCollectionView;
                
            }
            set
            {
                _filtersCollectionView = value;

                OnPropertyChanged();
            }
        }

        public void OnProgressChanged(object sender, ClientEventArgs args)
        {
            ProcessingProgress = Math.Min(100, int.Parse(args.Message));
        }

        public void GetOpenedPictPath()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Bitmap| *.bmp",
                CheckFileExists = true
            };
            if (dlg.ShowDialog() == true)
            {
                InitalPictPath = dlg.FileName;
            }
        }

        public void SaveResultWithDialog()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "temp",
                DefaultExt = ".bmp",
                Filter = "Bitmap(.bmp)|*.bmp"
            };
            if (dlg.ShowDialog() == true)
            {
                File.Copy(ResultPictPath, dlg.FileName);
            }
        }

        public void OnReceivedFilters(object sender, ClientEventArgs args)
        {
            FiltersCollectionView = new CollectionView(args.Message.Split(' '));
        }


        public void OnAnyError(object sender, ClientEventArgs args)
        {
            MessageBox.Show(args.Message, "Error");
        }

        public void OnPictProcessed(object sender, ClientEventArgs args)
        {
            ResultPictPath = Directory.GetCurrentDirectory() + "/" + args.Message;
            MessageBox.Show("Pict successfully processed. Click on it to compare", "Success!");
            IsInitalPictVisible = false;
            IsResultPictVisible = true;
            ProcessingProgress = 0;
        }

        public void ChangePictsVisibility(object sender, EventArgs args)
        {
            if (IsInitalPictVisible && ResultPictPath != null)
            {
                IsInitalPictVisible = false;
                IsResultPictVisible = true;
            }
            else
            {
                IsInitalPictVisible = true;
                IsResultPictVisible = false;
            }
        }

        public void UploadPict()
        {

            ResultPictPath = null;


            _model.UploadPict(new UploadingData(new Pict(InitalPictPath), (string)FiltersCollectionView.CurrentItem));
        }

    }
}