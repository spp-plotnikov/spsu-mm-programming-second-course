using System.Collections.Generic;
using System.Linq;

namespace UniversityRequest.HashSets
{
    public abstract class BaseHashSet<T>
    {
        protected List<T>[] table;
        protected int elementCount;

        protected BaseHashSet(int capacity)
        {
            table = Enumerable.Range(0, capacity).Select(i => new List<T>()).ToArray();
        }

        protected abstract void Acquire(T x);

        protected abstract void Release(T x);

        protected abstract bool IsNeedResize();

        protected abstract void Resize();

        public virtual bool Contains(T x)
        {
            Acquire(x);
            try
            {
                int myBucket = x.GetHashCode() % table.Length;
                return table[myBucket].Contains(x);
            }
            finally
            {
                Release(x);
            }
        }

        public virtual void Remove(T x)
        {
            Acquire(x);
            try
            {
                int myBucket = x.GetHashCode() % table.Length;
                if(table[myBucket].Contains(x))
                {
                    table[myBucket].Remove(x);
                }
            }
            finally
            {
                Release(x);
            }
        }

        public virtual void Add(T x)
        {
            Acquire(x);
            try
            {
                int bucket = x.GetHashCode() % table.Length;
                if (table[bucket].Contains(x))
                {
                    return;
                }
                else
                {
                    table[bucket].Add(x);
                }
            }
            finally
            {
                Release(x);
            }
            if (IsNeedResize())
                Resize();
        }
    }
}