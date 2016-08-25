using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class HashTable<T>
    {
        const int TableSize = 3;
        ElemHashTable<T>[] first = new ElemHashTable<T>[TableSize];

        public HashTable()
        {
            for (int i = 0; i < TableSize; i++)
            {
                first[i] = new ElemHashTable<T>(default(T));
            }
        }

        public void Add(T obj)
        {
            int hash = obj.GetHashCode() % TableSize;
            var next = first[hash];
            while (next.Next != null)
            {
                next = next.Next;
            }

            next.Next = new ElemHashTable<T>(obj);
        }

        public bool Search(T obj)
        {
            int hash = obj.GetHashCode() % TableSize;
            var next = first[hash];
            while (next.Next != null)
            {
                if (next.Next.Value.Equals(obj))
                {
                    return true;
                }
                next.Next = next.Next.Next;
            }
            return false;
        }

        public void Delete(T obj)
        {
            if (Search(obj))
            {
                int hash = obj.GetHashCode() % TableSize;
                var next = first[hash];
                while (!next.Next.Value.Equals(obj))
                {
                    next = next.Next;
                }
                while (next.Next != null)
                {
                    next.Next = next.Next.Next;
                }
            }
            else
            {
                Console.WriteLine("Element {0} isn`t found", obj);
            }
        }
    }
}
