using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum job { Student, Teacher }
public enum system { SystemCoarseGrained, SystemSpinLock }

namespace Decanat
{
    class Program
    {
        static void Main(string[] args)
        {
            ComparerSpeed csSSL = new ComparerSpeed(system.SystemSpinLock);
            csSSL.PrintSpeedResult();
            ComparerSpeed csSCG = new ComparerSpeed(system.SystemCoarseGrained);
            csSCG.PrintSpeedResult();
            Console.ReadLine();
        }
    }
}
