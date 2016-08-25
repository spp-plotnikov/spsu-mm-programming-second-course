using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baccarat
{
    sealed class Bank: Man
    {
        override internal int Score(ref int counter)
        {
            int s;
            s = (ChargePointsAfterDraw(ref counter) + ChargePointsAfterDraw(ref counter)) % 10;
            if (s < 5)
                s = (s + ChargePointsAfterDraw(ref  counter)) % 10;
            return s;
        }
    }
}
