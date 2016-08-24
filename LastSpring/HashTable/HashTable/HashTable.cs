using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class HashTable<T>
    {
        public enum ObjIsFound { No, Yes };
        static int TableSize = 3;
        StartHashTable<T>[] first = new StartHashTable<T>[TableSize];

        public HashTable()
        {
            for (int i = 0; i < TableSize; i++)
            {
                first[i] = new StartHashTable<T>(default(T));
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

            next.Next = new StartHashTable<T>(obj);
        }

        public ObjIsFound Search(T obj)
        {
            int hash = obj.GetHashCode() % TableSize;
            var next = first[hash].Next;
            while (next != null)
            {
                if (next.Value.Equals(obj))
                {
                    return ObjIsFound.Yes;
                }
                next = next.Next;
            }
            return ObjIsFound.No;
        }

        public void Delete(T obj)
        {
            if (Search(obj) == ObjIsFound.Yes)
            {
                int hash = obj.GetHashCode() % TableSize;
                var next = first[hash];
                while (next.Next.Value.Equals(obj) == false)
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
