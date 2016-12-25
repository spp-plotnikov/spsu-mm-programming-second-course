using System.Threading;

namespace Task3
{
    public class AtomicLock
    {
        private int locked = 0;
        public void Capture()
        {
            while(Interlocked.CompareExchange(ref locked, 1, 0) != 0);
        }
        public void Release()
        {
            Interlocked.Exchange(ref locked, 0);
        }
    }
}
