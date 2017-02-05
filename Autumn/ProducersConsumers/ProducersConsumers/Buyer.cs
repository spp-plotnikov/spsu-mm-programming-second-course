using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Buyer
    {
        private List<Request> takenRequest = new List<Request>();
        DateTime p2;
        public bool takeRequest(Request req, ref List<Request> listOffers, ref Mutex mutex)
        {
            //предполагаем, для Request значение поумолчанию - null.

            if (listOffers.Find(req.Equals) != null)
            {
                p2 = Program.pauseSeller.AddMilliseconds(500);
                mutex.WaitOne();
                if (!Program.theEnd)
                {
                    mutex.ReleaseMutex();
                    return false;
                }
                while ((DateTime.Now - Program.pauseSeller).CompareTo(p2 - Program.pauseSeller) <= 0)
                {
                }
                if (listOffers.Remove(listOffers.Find(req.Equals)))
                {
                    Console.WriteLine("Offer accepted! " + NameBuyer + " takes a lot " + req.NumberRequest);
                    takenRequest.Add(req);
                    NumberRequiredOffers--;
                    Program.pauseBuyer = DateTime.Now;
                    mutex.ReleaseMutex();
                    return true;
                }
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

        public void findOffer(ref List<Request> listOffers, ref Mutex mutex)
        {
            while (Program.theEnd)
            {
                bool _find = false;
                Request interestReq = new Request(0,0,0,0);
                while (!_find)
                {
                    for (int i = 0; i < listOffers.Count; i++)
                    {
                        if (!Program.theEnd)
                            break;
                        try
                        {
                            if (listOffers[i].Offer <= InterestOffer)
                            {
                                if (interestReq.RatingCreatorCompany <= listOffers[i].RatingCreatorCompany)
                                    interestReq = listOffers[i];

                                _find = true;
                                break;
                            }
                        }
                        catch
                        {
                        }
                        
                    }
                    if (!Program.theEnd)
                        break;
                    if (_find)
                        if (!takeRequest(interestReq,ref listOffers,ref mutex))
                            _find = false;
                }
                if (NumberRequiredOffers == 0)
                {
                    Console.WriteLine(NameBuyer + " living the market");
                    Thread.CurrentThread.Abort();
                }
            }
        }
    }
}
