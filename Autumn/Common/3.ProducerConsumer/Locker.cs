using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Locker
{
    private int isLocked = 0;

    public Locker() { }

    public void Lock()
    {
        while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0));
    }

    public void Release()
    {
        Interlocked.Exchange(ref isLocked, 0);
    }
}
