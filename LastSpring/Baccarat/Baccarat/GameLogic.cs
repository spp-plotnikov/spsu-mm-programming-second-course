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
            Player player = new Player();
            var bank = new Bank();
            for (int i = 1; i <= 40; i++)
            {
                int p = player.Score();
                int b = bank.Score();
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
    }
}
