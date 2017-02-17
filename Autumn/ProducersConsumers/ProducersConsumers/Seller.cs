using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Seller
    {
        private double[] _officialExchangeRate = new double[3] { 59.3137, 63.8156, 74.3438 };
        private List<string> _listOfNames = new List<string>() { "Lucky Corp.", "First Company", "Liberty Inc.", "Area Ltd." };

        public string NameCompany { get; private set; }
        public int RatingCompany { get; private set; }
        public int ItemNumberInBidding { get; private set; }

        private bool _sellerIsFinish = false;
        private int _amountRequest = 0;
        private int _threadCount = 0;

        internal Seller(int numberNameInList, int ratingCompany, int itemNumberInBidding)
        {
            NameCompany = _listOfNames[numberNameInList - 1];
            RatingCompany = ratingCompany;
            ItemNumberInBidding = itemNumberInBidding;
        }
        
        /// <param name="offer">Offer which can make the company. The difference with the official exchange rate.</param>
        /// <param name="exchangeRate">Currencies exchange in this offer</param>
        public void MakeRequest(TypesOfExchangeRate exchangeRate, List<Request> listOffers, Mutex mutex)
        {
            while (!_sellerIsFinish)
            {
                mutex.WaitOne();
                Random rand = new Random((int)DateTime.Now.Ticks);
                _amountRequest++;
                double _offer = rand.Next(300, 2000);
                _offer /= 1000;
                int numberRequest = ComposotionNumberRequest(_amountRequest, ItemNumberInBidding);
                Request request = new Request(numberRequest, RatingCompany, exchangeRate, _offer);
                if (_sellerIsFinish)
                {
                    mutex.ReleaseMutex();
                    break;
                }
                listOffers.Add(request);
                Console.WriteLine("New request! " + request.NumberRequest + " From " + NameCompany
                    + "     EUROtoRUB in exchange rate {0} ", _officialExchangeRate[1] + _offer);
                Console.WriteLine(DateTime.Now);
                mutex.ReleaseMutex();
                Thread.Sleep(5000);
            }
            _threadCount++;
            Console.WriteLine("Thread{0} finish work", _threadCount);
        }

        /// <summary>
        /// This function create the number request.
        /// </summary>
        private int ComposotionNumberRequest(int _amountRequest, int ItemNumberInBidding)
        {
            return Convert.ToInt32(_amountRequest.ToString() + ItemNumberInBidding.ToString());
        }

        /// <summary>
        /// Initialize and start this thread.
        /// </summary>
        public void ThreadInitializeAndStart(TypesOfExchangeRate exchangeRate,
                                                    List<Request> listOffers, Mutex mutex)
        {
            Thread thread = new Thread(() => MakeRequest(TypesOfExchangeRate.EUROtoRUB, listOffers, mutex));
            thread.Start();
        }

        /// <summary>
        /// Call when seller end work.
        /// </summary>
        public void FinishSeller()
        {
            _sellerIsFinish = true;
        }

    }
}
