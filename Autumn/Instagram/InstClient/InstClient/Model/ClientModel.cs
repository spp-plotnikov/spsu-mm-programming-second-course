﻿using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
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

            public void Notify(int progress)
            {
                _model.ProgressChanged?.Invoke(null, new ClientEventArgs(progress.ToString()));
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
                    _isAborted = false;
                    Pict pict = ((UploadingData) data).Picture;
                    string filter = ((UploadingData) data).Filter;
                    pict.PathToResult = GetValidName();

                    _client = new InstServiceClient(new InstanceContext(new NotificationHandler(this)),
                        "NetTcpBinding_IInstService");
                    var result = _client.EditPict(pict.PictBytes, filter);


                    pict.SavePict(result);
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

        private string GetValidName()
        {
            string result = "temp.bmp";
            int count = 0;
            while (true)
            {
                try
                {
                    string tempResult = "~" + count + result;
                    if (!File.Exists(tempResult)) return tempResult;
                    File.Open(tempResult, FileMode.Open).Close();

                    if (!UsedPicts.Contains(tempResult))
                    {
                        UsedPicts.Add(tempResult);
                    }
                    return tempResult;
                }
                catch (Exception)
                {
                    count++;
                }
            }
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