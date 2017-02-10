using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Buyer
    {
        private bool buyerIsFinish = false;
        private List<Request> _takenRequest = new List<Request>();
        public bool TakeRequest(Request req, List<Request> listOffers, Mutex mutex)
        {
            //предполагаем, для Request значение поумолчанию - null.

            if (listOffers.Find(req.Equals) != null)
            {
                if (buyerIsFinish)
                    return false;
                while ((DateTime.Now - Program.PauseBuyer).CompareTo(
                    Program.PauseBuyer.AddMilliseconds(500) - Program.PauseBuyer) <= 0)
                {
                    Thread.Sleep(Program.PauseBuyer.AddMilliseconds(500) - DateTime.Now);
                }
                if (listOffers.Remove(listOffers.Find(req.Equals)))
                {
                    Console.WriteLine("Offer accepted! " + NameBuyer + " takes a lot " + req.NumberRequest);
                    Console.WriteLine(DateTime.Now);
                    _takenRequest.Add(req);
                    NumberRequiredOffers--;
                    Program.SetPauseBuyerTime( DateTime.Now);
                    return true;
                }
            }
            return false;
        }

        List<string> listOfNames = new List<string>() { "Nick Valentine", "John Wick", "Bob Seger", "Rick Grimes" };
        public string NameBuyer { get; private set; }
        public double InterestOffer { get; private set; }
        public int ItemNumberInBidding { get; private set; }
        public TypesOfExchangeRate PairOfInterestRates { get; private set; }
        public int NumberRequiredOffers { get; private set; }


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
            while (!buyerIsFinish)
            {
                bool _find = false;
                Request interestReq = new Request(0,0,0,0);
                while (!_find)
                {
                    for (int i = 0; i < listOffers.Count; i++)
                    {
                        mutex.WaitOne();
                        if (buyerIsFinish)
                            break;
                        if (listOffers[i].Offer <= InterestOffer && listOffers.Count != 0
                            && interestReq.RatingCreatorCompany <= listOffers[i].RatingCreatorCompany)
                        {
                            interestReq = listOffers[i];
                            _find = true;
                        }
                        mutex.ReleaseMutex();
                    }
                    mutex.WaitOne();
                    if (buyerIsFinish)
                        break;
                    if (_find)
                        if (!TakeRequest(interestReq, listOffers, mutex))
                            _find = false;
                    mutex.ReleaseMutex();
                }
                if (NumberRequiredOffers == 0)
                {
                    Console.WriteLine(NameBuyer + " living the market");
                    break;
                }
            }
        }

        public void ThreadInitializeAndStart(List<Request> listOffers, Mutex mutex)
        {
            Thread thread = new Thread(() => FindOffer(listOffers, mutex));
            thread.Start();
        }

        public void FinishBuyer()
        {
            buyerIsFinish = true;
        }
    }
}
