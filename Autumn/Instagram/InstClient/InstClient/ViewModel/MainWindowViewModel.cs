using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ClientModel _model;
        private string _editAbortBtnName;

        public event ClientEventHandler ShowMessage;
        public event EventHandler OpenPictRequested;
        public event EventHandler SavePictRequested;

        public MainWindowViewModel(ClientModel model)
        {
            _model = model;
            model.ReceivedFilters += OnReceivedFilters;
            model.GotAnyError += OnAnyError;
            model.PictProcessed += OnPictProcessed;
            model.ProgressChanged += OnProgressChanged;
            OpenPict = new SimpleCommand(HandleOpenPictRequest);
            UpdateFilterList = new SimpleCommand(model.GetFilters);
            SendPict = new SimpleCommand(EditAbortPict);
            SaveResult = new SimpleCommand(HandleSavePictRequest);
            IsInitalPictVisible = true;
            IsResultPictVisible = false;
            EditAbortBtnName = "Send";
        }

        public string EditAbortBtnName
        {
            get
            {
                return _editAbortBtnName;
                
            }
            set
            {
                _editAbortBtnName = value;
                OnPropertyChanged();
            }
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



        public void OnReceivedFilters(object sender, ClientEventArgs args)
        {
            FiltersCollectionView = new CollectionView(args.Message.Split(' '));
        }


        public void OnAnyError(object sender, ClientEventArgs args)
        {
            ShowMessage?.BeginInvoke(this, new ClientEventArgs("Error" + args.Message), null, null);
        }

        public void OnPictProcessed(object sender, ClientEventArgs args)
        {
            if (args.Message != null)
            {
                ResultPictPath = Directory.GetCurrentDirectory() + "/" + args.Message;
                ShowMessage?.BeginInvoke(this, new ClientEventArgs("Success! Pict was successfully edited. Click on it to compare."), null, null);
                IsInitalPictVisible = false;
                IsResultPictVisible = true;
            }
            else
            {
                ProcessingProgress = 100;
            }
            EditAbortBtnName = "Send";
        }

        public void OnClosing(object sender, EventArgs args)
        {

            DeleteUsedPicts(_model.UsedPicts);
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

        private void DeleteUsedPicts(List<string> usedPicts)
        {

            try
            {
                foreach (var pictPath in usedPicts)
                {
                    File.Delete(pictPath);
                }
            }
            catch (Exception e)
            {
                //OnAnyError(this, new ClientEventArgs(e.ToString()));
            }
        }

        private void EditAbortPict()
        {
            if (EditAbortBtnName =="Send")
            {
                EditAbortBtnName = "Abort";
                ProcessingProgress = 0;

                try
                {
                    _model.EditPict(new UploadingData(new Pict(InitalPictPath), (string)FiltersCollectionView.CurrentItem));
                }
                catch (Exception e)
                {

                    ShowMessage?.BeginInvoke(this, new ClientEventArgs("Error " + e.ToString()), null, null);
                }
            }
            else
            {
                EditAbortBtnName = "Send";


                try
                {
                    _model.AbortPictEdit();
                }
                catch (Exception e)
                {

                    ShowMessage?.BeginInvoke(this, new ClientEventArgs("Error " + e.ToString()), null, null);
                }
            }


        }

        private void HandleOpenPictRequest()
        {
            OpenPictRequested?.Invoke(this, EventArgs.Empty); //, null, null);
        }

        private void HandleSavePictRequest()
        {
            SavePictRequested?.Invoke(this, EventArgs.Empty); //, null, null);
        }

    }
}