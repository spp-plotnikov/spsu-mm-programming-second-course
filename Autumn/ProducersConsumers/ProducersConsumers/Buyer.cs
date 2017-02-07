using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Buyer
    {
        private static bool stopTimeForRemove = false;
        private bool buyerIsFinish = false;
        private List<Request> _takenRequest = new List<Request>();
        private DateTime _pause;
        public bool TakeRequest(Request req, List<Request> listOffers, Mutex mutex)
        {
            //предполагаем, для Request значение поумолчанию - null.

            if (listOffers.Find(req.Equals) != null)
            {
                _pause = Program.PauseSeller.AddMilliseconds(500);
                mutex.WaitOne();
                stopTimeForRemove = true;
                if (buyerIsFinish)
                {
                    mutex.ReleaseMutex();
                    stopTimeForRemove = false;
                    return false;
                }
                while ((DateTime.Now - Program.PauseSeller).CompareTo(_pause - Program.PauseSeller) <= 0)
                {
                    Thread.Sleep(_pause - DateTime.Now);
                }
                if (listOffers.Remove(listOffers.Find(req.Equals)))
                {
                    Console.WriteLine("Offer accepted! " + NameBuyer + " takes a lot " + req.NumberRequest);
                    _takenRequest.Add(req);
                    NumberRequiredOffers--;
                    Program.SetPauseBuyerTime( DateTime.Now);
                    mutex.ReleaseMutex();
                    return true;
                }
                stopTimeForRemove = false; 
                mutex.ReleaseMutex();
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
            int _lastIndex=0;
            while (!buyerIsFinish)
            {
                bool _find = false;
                Request interestReq = new Request(0,0,0,0);
                while (!_find)
                {
                    for (int i = _lastIndex; i < listOffers.Count; i++)
                    {
                        while (stopTimeForRemove)
                        {
                            if ((DateTime.Now - Program.PauseSeller).CompareTo(
                                            Program.PauseSeller.AddMilliseconds(500) - Program.PauseSeller) <= 0)
                            {
                                Thread.Sleep(Program.PauseSeller.AddMilliseconds(500) - DateTime.Now);
                            }
                        }
                        if (buyerIsFinish)
                            break;
                        if (listOffers[i].Offer <= InterestOffer
                            && interestReq.RatingCreatorCompany <= listOffers[i].RatingCreatorCompany)
                        {
                            interestReq = listOffers[i];
                            _lastIndex = i;
                            _find = true;
                        }
                    }
                    if (buyerIsFinish)
                        break;
                    if (_find)
                        if (!TakeRequest(interestReq, listOffers, mutex))
                            _find = false;
                }
                if (NumberRequiredOffers == 0)
                {
                    Console.WriteLine(NameBuyer + " living the market");
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
