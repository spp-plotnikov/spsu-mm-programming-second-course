using System.Drawing;
using System.IO;
using System.ServiceModel;

namespace SpersyService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpersyService : IService
    {
        private Bitmap GrayScales(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int newValue = (image.GetPixel(j, i).R + image.GetPixel(j, i).G + image.GetPixel(j, i).B) / 3;
                    image.SetPixel(j, i, Color.FromArgb(newValue, newValue, newValue));
                }
            }
            return image;
        }
        public byte[] ProceedImage(byte[] bytes)
        {
            Bitmap image = (Bitmap)Image.FromStream(new MemoryStream(bytes));
            Bitmap result = GrayScales(image);
            ImageConverter converter = new ImageConverter();
            return (byte[])(new ImageConverter().ConvertTo(result, typeof(byte[])));
        }
    }
}
