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
        private int _str;
        public enum Strategy { AllOnPlayer = 1, AllOnBank= 2, AllOnDraw=3}
        public GameLogic(int str)
        {
            _str = str;
        }
        private int _losePlayer = 0, _draw = 0, _winPlayer = 0;
        
        public int Play()
        {
            int rate = 50;
            int startMoney = 1000;
            Player player = new Player();
            var bank = new Bank();
            for (int i = 1; i <= 40; i++)
            {
                int p = player.Score();
                int b = bank.Score();
                if (p > b)
                    _winPlayer++;
                if (p < b)
                    _losePlayer++;
                if (p == b)
                    _draw++;
                if((_draw + _losePlayer - _winPlayer >= 20 && _str == (int)Strategy.AllOnPlayer)
                    || (_winPlayer + _losePlayer - 9* _draw >= 20 && _str == (int)Strategy.AllOnDraw)
                    || (_draw + _winPlayer - _losePlayer >= 20 && _str == (int)Strategy.AllOnBank))
                {
                    Console.WriteLine("GAME OVER on {0} game", _draw + _losePlayer + _winPlayer);
                    return 0;
                }
            }
            if (_str == (int)Strategy.AllOnPlayer)    // Все ставки на победу игрока.
                return startMoney + rate * (-_losePlayer - _draw + _winPlayer);
            if (_str == (int)Strategy.AllOnBank)    //lose
                return startMoney + rate * (_losePlayer - _draw - _winPlayer);
            else
                return startMoney + rate * (-_losePlayer + 9 * _draw - _winPlayer);

        }  
    }
}
