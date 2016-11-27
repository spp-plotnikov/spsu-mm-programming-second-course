using System;
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
                try
                {
                    var handler = new NotificationHandler(this);
                    var client = new InstServiceClient(new InstanceContext(handler), "NetTcpBinding_IInstService");
                    var result = client.GetFilters();
                    client.Close();
                    ReceivedFilters?.Invoke(this, new ClientEventArgs(string.Join(" ", result)));
                }
                catch (Exception e)
                {
                    GotAnyError?.Invoke(this, new ClientEventArgs(e.ToString()));
                }
            });
            thread.Start();
        }

        public void EditPict(object data)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    Pict pict = ((UploadingData) data).Picture;
                    string filter = ((UploadingData) data).Filter;
                    pict.PathToResult = GetValidName();

                    var client = new InstServiceClient(new InstanceContext(new NotificationHandler(this)),
                        "NetTcpBinding_IInstService");
                    var result = client.EditPict(pict.PictBytes, filter);
                    client.Close();

                    pict.SavePict(result);
                    ProgressChanged?.Invoke(this, new ClientEventArgs("100"));
                    PictProcessed?.Invoke(this, new ClientEventArgs(pict.PathToResult));
                }
                catch (Exception e)
                {
                    GotAnyError?.Invoke(this, new ClientEventArgs(e.ToString()));
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
    }
}