using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baccarat
{
    abstract class Man
    {
        static Random rand = new Random((int)System.DateTime.Now.Millisecond);
        protected int ChargePointsAfterDraw() {
            int score = 0;
            int i=rand.Next(1,52);
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
            return score;
        }
        abstract internal int Score();
    }
}
