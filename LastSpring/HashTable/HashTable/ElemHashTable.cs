using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class ElemHashTable<T>
    {
        public ElemHashTable<T> Next;
        public T Value;

        public ElemHashTable(T obj)
        {
            this.Next = null;
            this.Value = obj;
        }
    }
}
