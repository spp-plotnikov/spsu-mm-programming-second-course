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
        protected int ChargePointsAfterDraw(ref int counter) {
            Random rand = new Random((int)System.DateTime.Now.Millisecond);
            byte[] b = new byte[20];
            rand.NextBytes(b);
            int score = 0;
            switch (b[counter] % 13)
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
            counter++;
            counter = counter % 20;
            return score;
        }
        abstract internal int Score(ref int counter);
    }
}
