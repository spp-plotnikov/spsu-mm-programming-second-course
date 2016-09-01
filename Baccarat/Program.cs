using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baccara
{
    class Program
    {
        static void Main()
        {

            int kolgamer = 2;
            int winner = -5;
            /*
             Console.WriteLine("Enter the number of players");
             kolgamer = Convert.ToInt32((Console.ReadLine());
             if (kolgamer>2) Console.WriteLine("You want to play with bots? Y N");
             if (N){}
             else{
            */

            double[,] winloser = new double[kolgamer, 2]; //массив выйгрышей/проигрышей в прошлой игре и количество выйгранных дененег
            winloser[0, 1] = 100;
            winloser[1, 1] = 100;
            //чтобы первая ставка у обоих была одинакова
            winloser[0, 0] = 5;
            winloser[1, 0] = 0;

            int[] gamercard = new int[kolgamer]; //массив суммы карт игроков
            int[,] wagers = new int[kolgamer, 3]; //массив ставок 2 - banker, 1 - player, 0 - tie;
            //  int deck = 210; //колода, играем, пока она не закончится      
            int deck = 0; // 40 ставок
            int bankir = 1; //кто будет банкиром
            int gamer = 0; //номер игрока  

            double medznachbot1 = 0, medznachbot2 = 0;

            for (int medznach = 0; medznach < 100; medznach++)
            {
                while (deck < 40) //6*36=216
                //while (winloser[0, 1] < 101 || winloser[1, 1] < 101)  //если ждать какого то конкретного выйгрыша
                {

                    //ставки/будем считать что его всё время увеличивают
                    //banker may choose to increase the bank to match;
                    Array.Clear(gamercard, 0, kolgamer);
                    BotMartin botplayer1 = new BotMartin();
                    BotDonald botplayer2 = new BotDonald();
                    botplayer1.game(wagers, winloser);
                    botplayer2.game(wagers, winloser);

                    /*
                     * для живых людей.
                     * Console.WriteLine("Ladies and gents, place your bets.");
                        while (gamer < kolgamer)
                        {
                          Console.WriteLine("Player " + gamer + ": banker, player, tie");
                          Console.WriteLine("Rate ");
                        }
                     * 
                     * ЗДЕСЬ ВЫБИРАЕТСЯ С БОЛЬШЕЙ СТАВКОЙ!!!!
                    */

                    //карты
                    deck++; //ставка
                    CardGame cardgame = new CardGame();

                    //ТАК КАК У НАС ДВА БОТА, ТО ВЫБИРАЕМ ПЕРВОГО ИГРОКОМ, ВТОРОГО БАНКИРОМ. ПОТОМ ПОМЕНЯЕМ. ИНАЧЕ
                    /*ФУНКЦИЯ ВЫБИРАЮЩАЯ БАНКИРА ИХ ИГРОКОВ*/


                    cardgame.GiveCard(gamercard, gamer);
                    // deck--; //взяли из колоды карту
                    cardgame.GiveCard(gamercard, gamer);//две же карты
                    ///   deck--; //взяли из колоды карту

                    cardgame.GiveCard(gamercard, bankir);
                    //   deck--; //взяли из колоды карту
                    cardgame.GiveCard(gamercard, bankir);//две же карты
                    //  deck--; //взяли из колоды карту


                    //проверка на натуральное очко
                    if (gamercard[gamer] > 7 || gamercard[bankir] > 7)
                    {
                        winner = cardgame.IsFinish(gamercard, gamer, bankir, winloser, wagers);

                    }
                    else
                    {

                        //выдача доп карт
                        if (gamercard[gamer] < 6)
                        {
                            cardgame.GiveCard(gamercard, gamer); //доп одна карта
                            //    deck--; //взяли из колоды карту
                        }

                        if (gamercard[bankir] < 6)
                        {
                            cardgame.GiveCard(gamercard, bankir); //доп одна карта
                            //     deck--; //взяли из колоды карту
                        }

                        //кто же победил
                        winner = cardgame.IsFinish(gamercard, gamer, bankir, winloser, wagers);
                    }

                    int flagwin = 0;
                    for (int gameri = 0; gameri < kolgamer; gameri++)
                    {
                        flagwin = 0;
                        for (int i = 0; i < 3; i++)
                            if (wagers[gameri, i] != 0 && i == winner)
                            {
                                winloser[gameri, 0] = 1; flagwin = 1;
                                if (i == 0) { winloser[gameri, 1] += 9 * wagers[gameri, i]; Console.WriteLine("Player " + (gameri + 1) + " wins " + 9 * wagers[gameri, i] + " He put on the tie"); }
                                if (i == 1) { winloser[gameri, 1] += wagers[gameri, i]; Console.WriteLine("Player " + (gameri + 1) + " wins " + wagers[gameri, i] + " He put on the player"); }
                                if (i == 2) { winloser[gameri, 1] += wagers[gameri, i] - 0.05 * wagers[gameri, i]; Console.WriteLine("Player " + (gameri + 1) + " wins " + (wagers[gameri, i] - 0.05 * wagers[gameri, i]) + " He put on the bank"); }
                            }
                        if (flagwin == 0) winloser[gameri, 0] = 0;
                    }

                    //меняем местами банкира и игрока.
                    if (winner != 2)
                    {
                        int b = bankir;
                        bankir = gamer;
                        gamer = b;
                    }

                    Console.WriteLine("It was " + deck + " bet in " + medznach);
                    Console.WriteLine("Player 1 with Martingale strategy  have " + winloser[0, 1]);
                    Console.WriteLine("Player 2 with Donald Nathanson-strategy  have " + winloser[1, 1]);
                    Console.WriteLine();
                }
                //10 раз по 40 ставок чтоб среднее найти
                medznachbot1 += winloser[0, 1];
                medznachbot2 += winloser[1, 1];
                deck = 0;
                Console.WriteLine();
                Console.WriteLine("Over last 40 bids received 1 bot " + winloser[0, 1]);
                Console.WriteLine("Over last 40 bids received 2 bot " + winloser[1, 1]);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("The average money remains at Player 1 with Martingale strategy after 40  Rates " + medznachbot1 / 100);
            Console.WriteLine("The average money remains at Player 2 with Donald Nathanson-strategy  after 40 Rates " + medznachbot2 / 100);
        }
    }
}
