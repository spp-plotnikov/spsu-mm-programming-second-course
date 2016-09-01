using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baccara
{
    class BotMartin : ParentBot
    {  //стратегия мартингейла
            public void game(int[,] wagers, double[,] winloser)
            {
                if (winloser[0, 0] == 0)
                    wagers[0, 2] *= 2;
                else
                    wagers[0, 2] = 10;

                winloser[0, 1] -= wagers[0, 2];
            }

    }
}
