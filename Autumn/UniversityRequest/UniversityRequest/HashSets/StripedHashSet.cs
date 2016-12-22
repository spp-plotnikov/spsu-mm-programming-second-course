using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UniversityRequest.HashSets
{
    public class StripedHashSet<T> : BaseHashSet<T>
    {
        private Mutex[] _locks;
        private int _threshold = 4;

        public StripedHashSet(int capacity) : base(capacity)
        {
            _locks = Enumerable.Range(0, capacity).Select(i => new Mutex()).ToArray();
        }

        protected override void Acquire(T x)
        {
            _locks[x.GetHashCode()%_locks.Length].WaitOne();
        }

        protected override void Release(T x)
        {
            _locks[x.GetHashCode()%_locks.Length].ReleaseMutex();
        }

        protected override bool IsNeedResize()
        {
            return (elementCount/table.Length > 4) ||
                   (Enumerable.Range(0, table.Length).Select(i => table[i].Count).Any(i => i > _threshold));
        }

        protected override void Resize()
        {
            int oldCapacity = table.Length;
            foreach (var @lock in _locks)
            {
                @lock.WaitOne();
            }
            try
            {
                if (oldCapacity != table.Length)
                {
                    return; // someone beat us to it
                }

                int newCapacity = 2 * oldCapacity;
                List<T>[] oldTable = table;
                table = Enumerable.Range(0, newCapacity).Select(i => new List<T>()).ToArray();

                foreach (var list in oldTable)
                {
                    foreach (var x in list)
                    {
                        table[x.GetHashCode()%table.Length].Add(x);
                    }
                }
            }
            finally
            {
                foreach (var @lock in _locks)
                {
                    @lock.ReleaseMutex();
                }
            }
        }
    }
}