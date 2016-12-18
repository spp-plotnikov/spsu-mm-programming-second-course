using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;
using ServiceInterface.InstSevrRef;

namespace InstClient.Model
{
    public class ClientModel
    {
        public event ClientEventHandler ReceivedFilters;
        public event ClientEventHandler GotAnyError;
        public event ClientEventHandler PictProcessed;
        public event ClientEventHandler ProgressChanged;
        public List<string> UsedPicts { get; set; }
        private InstServiceClient _client;
        private byte[] _pict;
        private bool _isAborted;
        private Mutex _abortFinished = new Mutex();

        public ClientModel()
        {
            UsedPicts = new List<string>();
        }

        public class NotificationHandler : IInstServiceCallback
        {
            private readonly ClientModel _model;

            public NotificationHandler(ClientModel model)
            {
                _model = model;
            }

            public void Notify(object progress)
            {
                _model.ProgressChanged?.Invoke(null, new ClientEventArgs(((int)progress).ToString()));
            }

            public void GetPict(byte[] pict)
            {
                _model._pict = pict;
            }
        }


        public void GetFilters()
        {
            Thread thread = new Thread(() =>
            {
                _client = null;
                try
                {
                    _client = new InstServiceClient(new InstanceContext(new NotificationHandler(this)), "NetTcpBinding_IInstService");
                    var result = _client.GetFilters();
                    _client.Close();
                    ReceivedFilters?.Invoke(this, new ClientEventArgs(string.Join(" ", result)));
                }
                catch (Exception e)
                {
                    GotAnyError?.Invoke(this, new ClientEventArgs(e.ToString()));
                }
                finally
                {
                    try
                    {
                        _client?.Close();
                    }
                    catch (Exception)
                    {
                        //ingo

                    }
                }
            });
            thread.Start();
        }

        public void EditPict(object data)
        {
            var thread = new Thread(() =>
            {
                _client = null;
                try
                {
                    _pict = null;
                    _isAborted = false;
                    Pict pict = ((UploadingData) data).Picture;
                    string filter = ((UploadingData) data).Filter;
                    pict.PathToResult = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bmp";

                    _client = new InstServiceClient(new InstanceContext(new NotificationHandler(this)),
                        "NetTcpBinding_IInstService");
                    _client.EditPict(pict.PictBytes, filter);

                    while (_pict == null)
                    {
                        
                    }

                    pict.SavePict(_pict);
                    ProgressChanged?.Invoke(this, new ClientEventArgs("100"));
                    PictProcessed?.Invoke(this, new ClientEventArgs(pict.PathToResult));
                }
                catch (Exception e)
                {
                    if(!_isAborted)
                        GotAnyError?.Invoke(this, new ClientEventArgs(e.ToString()));
                    PictProcessed?.Invoke(this, new ClientEventArgs(null));
                }
                finally
                {
                    try
                    {
                        _client?.Close();
                    }
                    catch (Exception)
                    {
                        //ingo
                    }
                }
            });

            thread.Start();
        }

        public void AbortPictEdit()
        {
            try
            {
                _isAborted = true;
                _client.Close();
            }
            catch (Exception)
            {
               //ign
            }
        }
    }
}