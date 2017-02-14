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
        private static Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            List<Request> listOffers = new List<Request>();
            Seller luckyCorp = new Seller(1, 5, 1);
            Seller firstCompany = new Seller(2, 0, 2);
            Seller libertyInc = new Seller(3, 10, 3);
            Seller areaLtd = new Seller(4, 9, 4);
            List<Seller> listSellers = new List<Seller>() { luckyCorp, firstCompany, libertyInc, areaLtd };

            Buyer nickValentine = new Buyer(1, 1, TypesOfExchangeRate.EUROtoRUB, 0.5, 2);
            Buyer johnWick = new Buyer(2, 2, TypesOfExchangeRate.EUROtoRUB, 0.4, 2);
            Buyer bobSeger = new Buyer(3, 3, TypesOfExchangeRate.EUROtoRUB, 1, 2);
            Buyer rickGrimes = new Buyer(4, 4, TypesOfExchangeRate.EUROtoRUB, 0.5, 2);
            List<Buyer> listBuyers = new List<Buyer>() { nickValentine, johnWick, bobSeger, rickGrimes };

            Console.WriteLine("If you want finish press something");
            for (int i = 0; i <= 3; i++)
            {
                Seller curSeller = listSellers[i];
                curSeller.ThreadInitializeAndStart(TypesOfExchangeRate.EUROtoRUB, listOffers, _mutex);
            }

            for (int i = 0; i <= 3; i++)
            {
                Buyer curBuyer = listBuyers[i];
                curBuyer.ThreadInitializeAndStart(listOffers, _mutex);
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
