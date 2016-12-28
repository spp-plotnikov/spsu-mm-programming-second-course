using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TimeVsImage
{
    public partial class MyTimeVsImage : Form
    {
        List<byte[]> _listOfImages;
        List<Client> _listOfClients;
        int _numberOfClients;
        int _numberOfImages;

        public MyTimeVsImage()
        {
            InitializeComponent();

            _numberOfClients = 5;
            _numberOfImages = 10;

            _listOfImages = new List<byte[]>();
            GetImageList();

            _listOfClients = new List<Client>();
            GetClientsList();

            Test();
        }

        public void GetImageList()
        {
            for (int i = 0; i < _numberOfImages; i++)
            {
                Bitmap image = new Bitmap((i + 1) * 200, (i + 1) * 200, PixelFormat.Format24bppRgb);
                using (Graphics graph = Graphics.FromImage(image))
                {
                    Rectangle ImageSize = new Rectangle(0, 0, (i + 1) * 200, (i + 1) * 200);
                    graph.FillRectangle(Brushes.White, ImageSize);
                }
                ImageConverter converter = new ImageConverter();
                byte[] imageArray = (byte[])converter.ConvertTo(image, typeof(byte[]));
                _listOfImages.Add(imageArray);
            }
        }

        public void GetClientsList()
        {
            for (int i = 0; i < _numberOfClients; i++)
            {
                _listOfClients.Add(new Client());
            }
        }

        public void Test()
        {
            foreach (var imageArray in _listOfImages)
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream = new MemoryStream(imageArray);
                Bitmap image = (Bitmap)Bitmap.FromStream(memoryStream);

                List<long> times = new List<long>();
                List<Task<long>> clientProcessingTasks = new List<Task<long>>();

                foreach (var client in _listOfClients)
                {
                    clientProcessingTasks.Add(Task.Run(() => client.SendAndGetImage(imageArray)));
                }
                Task.WaitAll(clientProcessingTasks.ToArray());

                clientProcessingTasks.ForEach(i => times.Add(i.Result));
                times.Sort();
                long sum = 0;
                times.ForEach(i => sum += i);
                timeVsImageChart.Series[0].Points.Add(new DataPoint(image.Height * image.Width, times[1]));
                timeVsImageChart.Series[1].Points.Add(new DataPoint(image.Height * image.Width, (long)(sum / (double)times.Count)));
            }
        }
    }
}
