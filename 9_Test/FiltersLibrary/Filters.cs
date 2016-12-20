/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/

namespace ImageConvolutionFilters
{


    public class SharpenFilter : ConvolutionFilterBase
    {
    
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double bias = 0.0;
        public override double Bias
        {
            get { return bias; }
        }

        private double[,] filterMatrix =
            new double[,] { { -1, -1, -1, },
                            { -1,  9, -1, },
                            { -1, -1, -1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

    public class BlurFilter : ConvolutionFilterBase
    {
      
        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double bias = 0.0;
        public override double Bias
        {
            get { return bias; }
        }

        private double[,] filterMatrix =
            new double[,] { { 0.0, 0.2, 0.0, },
                            { 0.2, 0.2, 0.2, },
                            { 0.0, 0.2, 0.2, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }


    public class SoftenFilter : ConvolutionFilterBase
    {
     
        private double factor = 1.0 / 8.0;
        public override double Factor
        {
            get { return factor; }
        }

        private double bias = 0.0;
        public override double Bias
        {
            get { return bias; }
        }

        private double[,] filterMatrix =
            new double[,] { { 1, 1, 1, },
                            { 1, 1, 1, },
                            { 1, 1, 1, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }

}