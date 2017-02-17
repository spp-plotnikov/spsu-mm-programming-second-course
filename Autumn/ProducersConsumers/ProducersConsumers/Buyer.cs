using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Buyer
    {
        private List<Request> _takenRequest = new List<Request>();
        private List<string> listOfNames = new List<string>() { "Nick Valentine", "John Wick", "Bob Seger", "Rick Grimes" };

        public TypesOfExchangeRate PairOfInterestRates { get; private set; }
        public string NameBuyer { get; private set; }
        public double InterestOffer { get; private set; }
        public int ItemNumberInBidding { get; private set; }
        public int NumberRequiredOffers { get; private set; }

        private bool _buyerIsFinish = false;


        /// <param name="interestOffer">The most expensive offer inetesting from Buyer.</param>
        internal Buyer(int numberNameInList, int item, TypesOfExchangeRate pairOfInterestRates,
                       double interestOffer, int numberRequireOffers)
        {
            NameBuyer = listOfNames[numberNameInList-1];
            ItemNumberInBidding = item;
            PairOfInterestRates = pairOfInterestRates;
            InterestOffer = interestOffer;
            NumberRequiredOffers = numberRequireOffers;
        }

        public void FindOffer( List<Request> listOffers, Mutex mutex)
        {
            while (!_buyerIsFinish)
            {
                bool _find = false;
                Request interestReq = new Request(0,0,0,0);
                while (!_find)
                {
                    for (int i = 0; i < listOffers.Count; i++)
                    {
                        mutex.WaitOne();
                        if (_buyerIsFinish)
                        {
                            mutex.ReleaseMutex();
                            break;
                        }
                        if (i < listOffers.Count && listOffers.Count != 0)
                        {
                            if (listOffers[i].Offer <= InterestOffer)
                            {
                                if (interestReq.RatingCreatorCompany <= listOffers[i].RatingCreatorCompany)
                                {
                                    interestReq = listOffers[i];
                                    _find = true;
                                }
                            }
                        }
                        mutex.ReleaseMutex();
                    }
                    mutex.WaitOne();
                    if (_buyerIsFinish)
                    {
                        mutex.ReleaseMutex();
                        break;
                    }
                    if (_find)
                        if (!TakeRequest(interestReq, listOffers, mutex)) 
                            _find = false;
                    mutex.ReleaseMutex();
                    if (_find)
                        Thread.Sleep(1000);
                }
                if (NumberRequiredOffers == 0)
                {
                    Console.WriteLine(NameBuyer + " living the market");
                    break;
                }
            }
        }

        public bool TakeRequest(Request req, List<Request> listOffers, Mutex mutex)
        {
            if (listOffers.Find(req.Equals) != null)            //Спросить нормально ли так делать с точки зрения CodeStyle
            {
                if (!_buyerIsFinish)
                {
                    if (listOffers.Remove(listOffers.Find(req.Equals)))
                    {
                        Console.WriteLine("Offer accepted! " + NameBuyer + " takes a lot " + req.NumberRequest);
                        Console.WriteLine(DateTime.Now);
                        _takenRequest.Add(req);
                        NumberRequiredOffers--;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ThreadInitializeAndStart(List<Request> listOffers, Mutex mutex)
        {
            Thread thread = new Thread(() => FindOffer(listOffers, mutex));
            thread.Start();
        }

        public void FinishBuyer()
        {
            _buyerIsFinish = true;
        }
    }
}
