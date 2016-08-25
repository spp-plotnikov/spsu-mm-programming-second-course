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
        public enum Strategy { AllOnPlayer = 1, AllOnBank= 2, AllOnDraw=3}
        public GameLogic(int str)
        {
            this.str = str;
        }
        
        public void Play()
        {
            int[] result = new int[3];  // 0    Lose Player
                                        // 1     Draw
                                        // 2     Win Player
            int rate = 50;
            int startMoney = 1000;
            int counter = 0;
            Player player = new Player();
            var bank = new Bank();
            for (int i = 1; i <= 40; i++)
            {
                int p = player.Score(ref counter);
                int b = bank.Score(ref counter);
                if (p > b)
                    result[0]++;
                if (p < b)
                    result[2]++;
                if (p == b)
                    result[1]++;
                if((result[1]+result[2]-result[0]>=20 && str == (int)Strategy.AllOnPlayer)
                    ||(result[0] + result[2] - 9*result[1] >= 20 && str == (int)Strategy.AllOnDraw)
                    || (result[1] + result[0] - result[2] >= 20 && str == (int)Strategy.AllOnBank))
                {
                    Console.WriteLine("GAME OVER on {0} game", result[1]+result[2]+result[0]);
                    break;
                }
            }
            if (str == (int)Strategy.AllOnPlayer)    // Все ставки на победу игрока.
                resultGame = startMoney + rate * (-result[2] - result[1] + result[0]);
            if (str == (int)Strategy.AllOnBank)    //lose
                resultGame = startMoney + rate * (result[2] - result[1] - result[0]);
            if (str == (int)Strategy.AllOnDraw)    //draw
                resultGame = startMoney + rate * (-result[2] + 9 * result[1] - result[0]);
        }  
    }
}
