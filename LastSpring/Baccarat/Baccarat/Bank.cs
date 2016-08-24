using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baccarat
{
    sealed class Bank: Man
    {
        internal int Score()
        {
            int s;
            s = (TakeCard() + TakeCard()) % 10; // Счёт после взятия двух карт
            if (s < 5)
            {
                s = (s + TakeCard()) % 10;
            }
            return s;
        }
    }
}
