using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducersConsumers
{     
    /// <summary>
    /// Request from binary option.
    /// </summary>
    class Request
    {
        public int NumberRequest { get; private set; }
        public int RatingCreatorCompany { get; private set; }
        public TypesOfExchangeRate ExchangeRateRequest { get; private set; }
        public double Offer { get; private set; }

        
        public Request(int numberRequest, int ratingCreatorCompany, TypesOfExchangeRate exchangeRate, double offer)
        {
            NumberRequest = numberRequest;
            RatingCreatorCompany = ratingCreatorCompany;
            ExchangeRateRequest = exchangeRate;
            Offer = offer;
        }

        public bool Equals (Request req)
        {
            if (req.NumberRequest == NumberRequest)
                return true;
            else
                return false;
        }
    }
}
