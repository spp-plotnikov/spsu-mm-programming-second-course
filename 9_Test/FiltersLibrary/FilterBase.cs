/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/

namespace ImageConvolutionFilters
{
    public abstract class ConvolutionFilterBase
    {
       
        public abstract double Factor
        {
            get;
        }

        public abstract double Bias
        {
            get;
        }

        public abstract double[,] FilterMatrix
        {
            get;
        }
    }
}
