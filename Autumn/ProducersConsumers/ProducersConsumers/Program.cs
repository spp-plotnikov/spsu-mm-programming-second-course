using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
public enum TypesOfExchangeRate
{
    USDtoRUB,
    EUROtoRUB,
    GBRtoRUB
}
namespace ProducersConsumers
{
    class Program
    {
        private static Mutex mutex = new Mutex();
        public static DateTime PauseBuyer { get; private set; }
        public static DateTime PauseSeller { get; private set; }


        public static void SetPauseSellerTime(DateTime time)
        {
            PauseSeller = time;
        }

        public static void SetPauseBuyerTime(DateTime time)
        {
            PauseBuyer = time;
        }

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            List<Request> listOffers = new List<Request>();
            Seller LuckyCorp = new Seller(1, 5, 1);
            Seller FirstCompany = new Seller(2, 0, 2);
            Seller LibertyInc = new Seller(3, 10, 3);
            Seller AreaLtd = new Seller(4, 9, 4);
            List<Seller> listSellers = new List<Seller>() { LuckyCorp, FirstCompany, LibertyInc, AreaLtd };

            Buyer NickValentine = new Buyer(1, 1, TypesOfExchangeRate.EUROtoRUB, 0.5, 2);
            Buyer JohnWick = new Buyer(2, 2, TypesOfExchangeRate.EUROtoRUB, 0.4, 2);
            Buyer BobSeger = new Buyer(3, 3, TypesOfExchangeRate.EUROtoRUB, 1, 2);
            Buyer RickGrimes = new Buyer(4, 4, TypesOfExchangeRate.EUROtoRUB, 0.5, 2);
            List<Buyer> listBuyers = new List<Buyer>() { NickValentine, JohnWick, BobSeger, RickGrimes };

            Console.WriteLine("If you want finish press something");
            for (int i = 0; i <= 3; i++)
            {
                Seller curSeller = listSellers[i];
                curSeller.ThreadInitializeAndStart(TypesOfExchangeRate.EUROtoRUB, listOffers, mutex);
            }

            for (int i = 0; i <= 3; i++)
            {
                Buyer curBuyer = listBuyers[i];
                curBuyer.ThreadInitializeAndStart(listOffers, mutex);
            }

            Console.ReadKey();

            for(int i=0; i <= 3; i++)
            {
                listBuyers[i].FinishBuyer();
                listSellers[i].FinishSeller();
            }
            Console.WriteLine("Program finished work!");

        }
    }
}
