﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducersConsumers
{
    class Seller
    {
        private bool sellerIsFinish = false;
        private int _threadCount = 0;
        List<string> listOfNames = new List<string>() { "Lucky Corp.", "First Company", "Liberty Inc.", "Area Ltd." };
        public string NameCompany { get; private set; }
        public int RatingCompany { get; private set; }
        public int ItemNumberInBidding { get; private set; }
        private int _amountRequest = 0;
        private double[] OfficialExchangeRate = new double[3] { 59.3137, 63.8156, 74.3438 };

        internal Seller(int numberNameInList, int ratingCompany, int itemNumberInBidding)
        {
            NameCompany = listOfNames[numberNameInList - 1];
            RatingCompany = ratingCompany;
            ItemNumberInBidding = itemNumberInBidding;
        }

        DateTime _pause;
        /// <param name="offer">Offer which can make the company. The difference with the official exchange rate.</param>
        /// <param name="exchangeRate">Currencies exchange in this offer</param>
        public void makeRequest(TypesOfExchangeRate exchangeRate, List<Request> listOffers, Mutex mutex)
        {
            while (!sellerIsFinish)
            {
                mutex.WaitOne();
                Random rand = new Random((int)DateTime.Now.Ticks);
                _amountRequest++;
                double offer = rand.Next(300, 2000);
                offer /= 1000;
                int numberRequest = ComposotionNumberRequest(_amountRequest, ItemNumberInBidding);
                Request request = new Request(numberRequest, RatingCompany, exchangeRate, offer);
                _pause = Program.PauseSeller.AddMilliseconds(2000);
                while ((DateTime.Now - Program.PauseSeller).CompareTo(_pause - Program.PauseSeller) <= 0)
                {
                    Thread.Sleep(_pause - DateTime.Now);
                }
                if (sellerIsFinish)
                {
                    mutex.ReleaseMutex();
                    break;
                }
                listOffers.Add(request);
                Console.WriteLine("New request! " + request.NumberRequest + " From " + NameCompany
                    + "     EUROtoRUB in exchange rate {0} ", OfficialExchangeRate[1] + offer);
                Program.SetPauseSellerTime(DateTime.Now);
                mutex.ReleaseMutex();
            }
            _threadCount++;
            Console.WriteLine("Thread{0} finish work", _threadCount);
        }

        private int ComposotionNumberRequest(int _amountRequest, int ItemNumberInBidding)
        {
            return Convert.ToInt32(_amountRequest.ToString() + ItemNumberInBidding.ToString());
        }

        public void ThreadInitializeAndStart(TypesOfExchangeRate exchangeRate,
                                                    List<Request> listOffers, Mutex mutex)
        {
            Thread thread = new Thread(() => makeRequest(TypesOfExchangeRate.EUROtoRUB, listOffers, mutex));
            thread.Start();
        }

        public void FinishSeller()
        {
            sellerIsFinish = true;
        }

    }
}
