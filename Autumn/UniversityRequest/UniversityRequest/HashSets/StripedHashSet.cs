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
            return (ElementCount/Table.Length > 4) ||
                   (Enumerable.Range(0, Table.Length).Select(i => Table[i].Count).Any(i => i > _threshold));
        }

        protected override void Resize()
        {
            int oldCapacity = Table.Length;
            foreach (var @lock in _locks)
            {
                @lock.WaitOne();
            }
            try
            {
                if (oldCapacity != Table.Length)
                {
                    return; // someone beat us to it
                }

                int newCapacity = 2 * oldCapacity;
                List<T>[] oldTable = Table;
                Table = Enumerable.Range(0, newCapacity).Select(i => new List<T>()).ToArray();

                foreach (var list in oldTable)
                {
                    foreach (var x in list)
                    {
                        Table[x.GetHashCode()%Table.Length].Add(x);
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