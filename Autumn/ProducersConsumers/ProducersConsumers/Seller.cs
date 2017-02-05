using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Seller
    {
        List<string> listOfNames = new List<string>() {"Lucky Corp.","First Company","Liberty Inc.","Area Ltd."};
        public string NameCompany { get; private set;}
        public int RatingCompany { get; private set; }
        public int ItemNumberInBidding { get; private set; }
        private int _amountRequest = 0;
        
        internal Seller(int numberNameInList, int ratingCompany, int itemNumberInBidding)
        {
            NameCompany = listOfNames[numberNameInList-1];
            RatingCompany = ratingCompany;
            ItemNumberInBidding = itemNumberInBidding;
        }

        DateTime p2;
        /// <param name="offer">Offer which can make the company. The difference with the official exchange rate.</param>
        /// <param name="exchangeRate">Currencies exchange in this offer</param>
        public void makeRequest(TypesOfExchangeRate exchangeRate, ref List<Request> listOffers, ref Mutex mutex)
        {
            while (Program.theEnd)
            {
                mutex.WaitOne();
                Random rand = new Random((int)DateTime.Now.Ticks);
                _amountRequest++;
                double offer = rand.Next(300, 2000);
                //double offer = randFromZarkhidze(300, 2000);
                offer /= 1000;
                int numberRequest = composotionNumberRequest(_amountRequest, ItemNumberInBidding);
                Request request = new Request(numberRequest, RatingCompany, exchangeRate, offer);
                p2 = Program.pauseSeller.AddMilliseconds(2000);
                while ((DateTime.Now - Program.pauseSeller).CompareTo(p2 - Program.pauseSeller) <=0)
                {
                }
                if (!Program.theEnd)
                {
                    mutex.ReleaseMutex();
                    break;
                }
                listOffers.Add(request);
                Console.WriteLine("New request! " + request.NumberRequest + " From " + NameCompany
                    + "     EUROtoRUB in exchange rate {0} ", Program.OfficialExchangeRate[1] + offer);
                Program.pauseSeller = DateTime.Now;
                mutex.ReleaseMutex();
            }
            Program.threadCount++;
            Console.WriteLine("Thread{0} finish work", Program.threadCount);
        }

        private int composotionNumberRequest(int _amountRequest, int ItemNumberInBidding)
        {
            return Convert.ToInt32(_amountRequest.ToString() + ItemNumberInBidding.ToString());
        }

        private double randFromZarkhidze (double minValue, double maxValue)
        {
            return (DateTime.Now.Millisecond) % (maxValue - minValue) + minValue;
        }

    }
}
