using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TimeVsClients
{
    public partial class MyTimeVsClients : Form
    {
        int _numberOfClients;
        bool _break;
        List<Client> _listOfClients;
        byte[] _imageArray;

        public MyTimeVsClients()
        {
            InitializeComponent();

            _numberOfClients = 1;
            _break = false;
            _listOfClients = new List<Client>();
            _imageArray = GetImage();

            Test();
            maxClientsLabel.Text = String.Format("Max number of clients: {0}", _numberOfClients);
        }

        public byte[] GetImage()
        {
            Bitmap image = new Bitmap(256, 256);
            using (Graphics graph = Graphics.FromImage(image))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 256, 256);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }

        public void GetClientsList()
        {
            _listOfClients.Clear();
            for (int i = 0; i < _numberOfClients; i++)
            {
                _listOfClients.Add(new Client());
            }
        }

        public void Test()
        {
            while (!_break)
            {
                try
                {
                    long time = 0;
                    GetClientsList();
                    List<Task<long>> clientProcessingTasks = new List<Task<long>>();

                    foreach (var client in _listOfClients)
                    {
                        clientProcessingTasks.Add(Task.Run(() => client.SendAndGetImage(_imageArray)));
                    }
                    Task.WaitAll(clientProcessingTasks.ToArray());

                    foreach (var task in clientProcessingTasks)
                    {
                        if (task.Result == 0)
                        {
                            _break = true;
                            break;
                        }
                    }
                    if (!_break)
                    {
                        clientProcessingTasks.ForEach(i => time += i.Result);
                        timeVsClientsChart.Series[0].Points.Add(new DataPoint(_numberOfClients, time));
                        _numberOfClients++;
                    }
                }
                catch (Exception)
                {
                    _break = true;
                }
            }
        }
    }
}

