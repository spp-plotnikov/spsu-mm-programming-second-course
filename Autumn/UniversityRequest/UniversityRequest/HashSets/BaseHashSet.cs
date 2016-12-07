using System.Collections.Generic;
using System.Linq;

namespace UniversityRequest.HashSets
{
    public abstract class BaseHashSet<T>
    {
        protected List<T>[] Table;
        protected int ElementCount;

        protected BaseHashSet(int capacity)
        {
            Table = Enumerable.Range(0, capacity).Select(i => new List<T>()).ToArray();
        }

        protected abstract void Acquire(T x);

        protected abstract void Release(T x);

        protected abstract bool Policy();

        protected abstract void Resize();

        public bool Contains(T x)
        {
            Acquire(x);
            try
            {
                int myBucket = x.GetHashCode() % Table.Length;
                return Table[myBucket].Contains(x);
            }
            finally
            {
                Release(x);
            }
        }

        public void Remove(T x)
        {
            Acquire(x);
            try
            {
                int myBucket = x.GetHashCode() % Table.Length;
                if(Table[myBucket].Contains(x))
                {
                    Table[myBucket].Remove(x);
                }
            }
            finally
            {
                Release(x);
            }
        }

        public void Add(T x)
        {
            Acquire(x);
            try
            {
                int bucket = x.GetHashCode() % Table.Length;
                if (Table[bucket].Contains(x))
                {
                    return;
                }
                else
                {
                    Table[bucket].Add(x);
                }
            }
            finally
            {
                Release(x);
            }
            if (Policy())
                Resize();
        }
    }
}