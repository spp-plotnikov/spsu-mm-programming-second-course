using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public static class Locker
{
    public static int isLocked = 0;

    public static void Lock()
    {
        while (0 != Interlocked.CompareExchange(ref Locker.isLocked, 1, 0)) ;
    }

    public static void Release()
    {
        Interlocked.Exchange(ref Locker.isLocked, 0);
    }
}
