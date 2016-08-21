using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baccarat
{
    class GameLogic
    {
        int str;
        public int resultGame;
        
        public GameLogic(int str)
        {
            this.str = str;
        }
        
        public void Game()
        {
            int[] result = new int[3];  // 0    Lose Player
                                        // 1     Draw
                                        // 2     Win Player
            int rate = 50;
            int startMoney = 1000;
            for (int i = 1; i <= 40; i++)
            {
                int p = Score("player");
                int b = Score("bank");
                if (p > b)
                    result[0]++;
                if (p < b)
                    result[2]++;
                if (p == b)
                    result[1]++;
            }
            if (str == 1)    // Все ставки на победу игрока.
                resultGame = startMoney + rate * (-result[2] - result[1] + result[0]);
            if (str == 2)    //lose
                resultGame = startMoney + rate * (result[2] - result[1] - result[0]);
            if (str == 3)    //draw
                resultGame = startMoney + rate * (-result[2] + 9 * result[1] - result[0]);
        }

        int Score(string a)
        {
            int s;
            s = (TakeCard() + TakeCard()) % 10; // Счёт после взятия двух карт
            if (a.Equals("player") && (s < 6))  //Добор ещё одной при определённом условии
            {
                s = (s + TakeCard()) % 10;
            }
            if (a.Equals("bank") && (s < 5))
            {
                s = (s + TakeCard()) % 10;
            }

            return s;
        }


        static int TakeCard()  // В игре будет взято максимум 6 карт, при наличии 6 колод взятые карты могут быть
        {               // одинаковыми, поэтому метод TakeCard универсален для любого взятия карты
            Random rand = new Random();
            int i = rand.Next(1, 52);
            int score = 0;
            switch (i % 13)
            {
                case 0: score = 1; break;//туз
                case 1: score = 2; break;//двойка
                case 2: score = 3; break;
                case 3: score = 4; break;
                case 4: score = 5; break;
                case 5: score = 6; break;
                case 6: score = 7; break;
                case 7: score = 8; break;
                case 8: score = 9; break;
                case 9: break;
                case 10: break;
                case 11: break;
                case 12: break;
            }
            Thread.Sleep(15);
            return score;
        }
    }
}
