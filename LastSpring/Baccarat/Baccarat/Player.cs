using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baccarat
{
    sealed class Player: Man
    {
        internal int Score()
        {
            int s;
            s = (TakeCard() + TakeCard()) % 10; // Счёт после взятия двух карт
            if (s < 6)  //Добор ещё одной при определённом условии
            {
                s = (s + TakeCard()) % 10;
            }
            return s;
        }
    }
}
