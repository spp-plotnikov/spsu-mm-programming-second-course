using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baccarat
{
    sealed class Bank: Man
    {
        override internal int Score()
        {
            int s;
            s = (ChargePointsAfterDraw() + ChargePointsAfterDraw()) % 10;
            if (s < 5)
                s = (s + ChargePointsAfterDraw()) % 10;
            return s;
        }
    }
}
