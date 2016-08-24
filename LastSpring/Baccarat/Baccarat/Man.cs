using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baccarat
{
    class Man
    {
        protected int TakeCard()  // В игре будет взято максимум 6 карт, при наличии 6 колод взятые карты могут быть
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
