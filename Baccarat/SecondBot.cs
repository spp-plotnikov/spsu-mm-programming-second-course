using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baccara
{
    //стратегия дональда-натансона
    class BotDonald : ParentBot
    {
        public void game(int[,] wagers, double[,] winloser)
        {
            if (winloser[1, 0] == 0)
                wagers[1, 2] += 10;
            else
                wagers[1, 2] = 10;
            winloser[1, 1] -= wagers[1, 2];
        }
    }
}