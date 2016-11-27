using System;
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

        public void UploadPict(object data)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    Pict pict = ((UploadingData)data).Picture;
                    string filter = ((UploadingData)data).Filter;
                    pict.PathToResult = GetValidName();

                    /*byte[] picture = new byte[pict.PictBytes.Length]; //JUST DON'T ASK

                    for (int i = 0; i < pict.PictBytes.Length; i++)
                    {
                        picture[i] = pict.PictBytes[i];
                    }*/


                    var client = new InstServiceClient(new InstanceContext(new NotificationHandler(this)), "NetTcpBinding_IInstService");
                    var result = client.ProcessPict(pict.PictBytes, filter);
                    client.Close();

                    pict.SavePict(result);
                    PictProcessed?.Invoke(this, new ClientEventArgs(pict.PathToResult));
                }
                catch (Exception e)
                {
                    GotAnyError?.Invoke(this, new ClientEventArgs(e.ToString()));
                }
            });
            thread.Start();
        }

        private static string GetValidName()
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