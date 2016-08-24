using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class StartHashTable<T>
    {
        public StartHashTable<T> Next;
        public T Value;

        public StartHashTable(T obj)
        {
            this.Next = null;
            this.Value = obj;
        }
    }
}
