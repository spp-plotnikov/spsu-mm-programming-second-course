using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    static class FiltersWImplementation
    {
        private static Filter jackalFilter = new Filter("Jackal", JackalFilterImplementation);
        private static Filter invertFilter = new Filter("Invert", InvertFilterImplementation);
        private static Filter redFilter = new Filter("Red", RedFilterImplementation);
        public static List<Filter> filterList = new List<Filter>() { invertFilter, redFilter, jackalFilter };

        static public List<Filter> ListOfFilters
        {
            get
            {
                return filterList;
            }
        }

        static private Bitmap JackalFilterImplementation(Bitmap srcImage, ref double progress)
        {
            progress = 0;
            int compressionRatio = 20;
            double step = 1 / ((double)srcImage.Width) * compressionRatio;
            long sqCompressionRatio = compressionRatio * compressionRatio;
            Bitmap result = new Bitmap(srcImage.Width - srcImage.Width % compressionRatio, srcImage.Height - srcImage.Height % compressionRatio);
            for (int x = 0; x < srcImage.Width - compressionRatio; x += compressionRatio)
            {
                for (int y = 0; y < srcImage.Height - compressionRatio; y += compressionRatio)
                {
                    long sumOfR = 0;
                    long sumOfG = 0;
                    long sumOfB = 0;
                    for (int i = 0; i < compressionRatio; i++)
                    {
                        if (jackalFilter.Cancelled) { return result; }
                        for (int j = 0; j < compressionRatio; j++)
                        {
                            sumOfR += srcImage.GetPixel(x + i, y + j).R;
                            sumOfG += srcImage.GetPixel(x + i, y + j).G;
                            sumOfB += srcImage.GetPixel(x + i, y + j).B;
                        }                        
                    }
                    Color newColor = Color.FromArgb((int)(sumOfR / sqCompressionRatio), (int)(sumOfG / sqCompressionRatio), (int)(sumOfB / sqCompressionRatio));
                    for (int i = 0; i < compressionRatio; i++)
                    {
                        for (int j = 0; j < compressionRatio; j++)
                        {
                            result.SetPixel(x + i, y + j, newColor);                            
                        }                        
                    }
                }
                progress += step;
            }
            return result;
        }

        static private Bitmap InvertFilterImplementation(Bitmap srcImage, ref double progress)
        {
            progress = 0;
            double step = 1.0 / srcImage.Width;
            Bitmap result = new Bitmap(srcImage);
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    if (invertFilter.Cancelled) { return result; }
                    Color pixelColor = result.GetPixel(i, j);
                    Color newColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                    result.SetPixel(i, j, newColor);                    
                }
                progress += step;                
            }
            return result;
        }

        static private Bitmap RedFilterImplementation(Bitmap srcImage, ref double progress)
        {
            progress = 0;
            double step = 1.0 / srcImage.Width;
            Bitmap result = new Bitmap(srcImage);
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    if (redFilter.Cancelled) { return result; }
                    Color pixelColor = result.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    result.SetPixel(i, j, newColor);
                }
                progress += step;
            }
            return result;
        }
    }
}
