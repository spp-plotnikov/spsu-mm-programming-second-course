using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baccara
{
    class CardGame
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        public void GiveCard(int[] gamercard, int gamer)
        {
            int temp = rand.Next(2, 15);
            if (temp > 9 && temp != 15) temp = 0;
            if (temp == 15) temp = 1;
            gamercard[gamer] += temp;

            while (gamercard[gamer] > 9) gamercard[gamer] -= 10;
        }


        public int IsFinish(int[] gamercard, int gamer, int bankir, double[,] winloser, int[,] wagers)
        {
            if (gamercard[gamer] == gamercard[bankir])
            {
                Console.WriteLine("New Game"); Console.WriteLine("Tie, all bets are returned");
                for (int i = 0; i < 3; i++)
                {
                    winloser[gamer, 1] += wagers[gamer, i];
                    winloser[bankir, 1] += wagers[bankir, i];
                }
                return 0;
            }

            if (gamercard[gamer] > gamercard[bankir])
            {
                Console.WriteLine("New Game"); Console.WriteLine("Win player " + (gamer + 1) + " the bet is returned to the player");
                for (int i = 0; i < 3; i++)
                    winloser[gamer, 1] += wagers[gamer, i];
                return 1;
            }

            if (gamercard[gamer] < gamercard[bankir])
            {
                Console.WriteLine("New Game"); Console.WriteLine("Win bankir, player  " + (bankir + 1) + " the bet is returned to the bankir");
                for (int i = 0; i < 3; i++)
                    winloser[bankir, 1] += wagers[bankir, i];
                return 2;
            }
            return -2;
            //конец проверки
        }
    }
}