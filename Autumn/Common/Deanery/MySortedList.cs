using System.Collections.Generic;

namespace Deanery
{
    class MySortedList<T> : List<T>
    {
        public new void Add(T item)
        {
            Insert(~BinarySearch(item), item);
        }

        public new bool Remove(T item)
        {
            var index = this.BinarySearch(item);
            if (index < 0) return false;
            this.RemoveAt(index);
            return true;
        }
                
        public new bool Contains(T item)
        {
            return this.BinarySearch(item) >= 0;
        }
    }
}
