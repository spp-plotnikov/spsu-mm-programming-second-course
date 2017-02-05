using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
public enum TypesOfExchangeRate { USDtoRUB, EUROtoRUB, GBRtoRUB }

namespace ProducersConsumers
{
    class Program
    {
        public static bool theEnd = true;
        public static int threadCount = 0;
        public static Mutex mutex = new Mutex();
        public static DateTime pauseBuyer = DateTime.Now;
        public static DateTime pauseSeller = DateTime.Now;
        public static double[] OfficialExchangeRate = new double[3] { 59.3137, 63.8156, 74.3438 };

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            List<Request> listOffers = new List<Request>();
            Seller LuckyCorp = new Seller(1, 5, 1);
            Seller FirstCompany = new Seller(2, 0, 2);
            Seller LibertyInc = new Seller(3, 10, 3);
            Seller AreaLtd = new Seller(4, 9, 4);
            List<Seller> listSellers = new List<Seller>() { LuckyCorp, FirstCompany, LibertyInc, AreaLtd };

            Buyer NickValentine = new Buyer(1, 1, (TypesOfExchangeRate)1, 0.5, 2);
            Buyer JohnWick = new Buyer(2, 2, (TypesOfExchangeRate)1, 0.4, 2);
            Buyer BobSeger = new Buyer(3, 3, (TypesOfExchangeRate)1, 1, 2);
            Buyer RickGrimes = new Buyer(4, 4, (TypesOfExchangeRate)1, 0.5, 2);
            List<Buyer> listBuyers = new List<Buyer>() { NickValentine, JohnWick, BobSeger, RickGrimes };

            Console.WriteLine("If you want finish press something");
            for (int i = 0; i <= 3; i++)
            {
                Seller curSeller = listSellers[i];
                Thread thread = new Thread(() => curSeller.makeRequest((TypesOfExchangeRate)1, ref listOffers, ref mutex));
                thread.Start();
            }

            for (int i = 0; i <= 3; i++)
            {
                Buyer curBuyer = listBuyers[i];
                Thread thread = new Thread(() => curBuyer.findOffer(ref listOffers, ref mutex));
                thread.Start();
            }
            for (int i = 0; i < listOffers.Count; i++)
                Console.WriteLine(listOffers[i].NumberRequest);
            

            Console.ReadKey();
            theEnd = false;
            Console.WriteLine("Program finished work!");

        }
    }
}
