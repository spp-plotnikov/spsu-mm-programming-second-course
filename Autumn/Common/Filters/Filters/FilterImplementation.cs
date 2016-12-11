using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    static class FilterImplementation
    {
        static private Filter invertFilter = new Filter("Invert", InvertFilterImplementation);
        static private Filter refFilter = new Filter("Red", RedFilterImplementation);

        static public List<Filter> FilterList
        {
            get
            {
                return new List<Filter> { invertFilter, refFilter };
            }
        }

        static public List<string> FilterNames
        {
            get
            {
                return new List<string> { "Invert", "Red" };
            }
        }

        static private Bitmap InvertFilterImplementation(Bitmap srcImage, ref long pixelCounter)
        {
            Bitmap result = new Bitmap(srcImage);
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    Color pixelColor = result.GetPixel(i, j);
                    Color newColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                    result.SetPixel(i, j, newColor);
                    pixelCounter++;
                }
            }
            return result;
        }

        static private Bitmap RedFilterImplementation(Bitmap srcImage, ref long pixelCounter)
        {
            Bitmap result = new Bitmap(srcImage);
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    Color pixelColor = result.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    result.SetPixel(i, j, newColor);
                    pixelCounter++;
                }
            }
            return result;
        }
    }
}
