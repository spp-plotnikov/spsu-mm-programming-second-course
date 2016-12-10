using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam_Lab
{
    class LazyList<T>
    {
        private class Node
        {
            public readonly T Item;
            public readonly int Key;
            public Node Next;
            public bool Marked;
            private Mutex _lock = new Mutex();
            public Node (T item)
            {
                Item = item;
                Key = item.GetHashCode();
                Next = null;
                Marked = false;
            }
            public void Lock ()
            {
                _lock.WaitOne();
            }
            public void Unlock()
            {
                _lock.ReleaseMutex();
            }
        }

        private Node _head;
        private Node _tail;

        public LazyList()
        {
            _head = new Node(default(T));
            _tail = new Node(default(T));
            _head.Next = _tail;
            _tail.Next = null;
        }

        private bool Validate(Node pred, Node curr)
        {
            return !pred.Marked && !curr.Marked && pred.Next == curr;
        }

        public bool Add (T item)
        {
            int key = item.GetHashCode();
            while (true)
            {
                Node pred = _head;
                Node curr = _head.Next;
                while (curr != _tail && curr.Key < key)
                {
                    pred = curr;
                    curr = curr.Next;
                }
                pred.Lock();
                try
                {
                    curr.Lock();
                    try
                    {
                        if (Validate(pred, curr))
                        {
                            if (curr.Item.Equals(item))
                                return false;
                            else
                            {
                                Node node = new Node(item);
                                node.Next = curr;
                                pred.Next = node;
                                return true;
                            }
                        }
                    }
                    finally
                    {
                        curr.Unlock();
                    }                    
                }
                finally
                {
                    pred.Unlock();
                }
            }
        }

        public bool Remove (T item)
        {
            int key = item.GetHashCode();
            while (true)
            {
                Node pred = _head;
                Node curr = _head.Next;
                while (curr != _tail && curr.Key < key)
                {
                    pred = curr;
                    curr = curr.Next;
                }
                pred.Lock () ;
                try
                {
                    curr.Lock () ;
                    try
                    {
                        if (Validate(pred, curr))
                        {
                            if (!curr.Item.Equals(item) || curr == _tail)
                                return false;
                            else
                            {
                                curr.Marked = true;
                                pred.Next = curr.Next;
                                return true;
                            }
                        }
                    }
                    finally
                    {
                        curr.Unlock();
                    }
                }
                finally
                {
                    pred.Unlock();
                }
            }
        }

        public bool Contains (T item)
        {
            int key = item.GetHashCode();
            Node curr = _head;
            while (curr != _tail && curr.Key < key)
                curr = curr.Next;
            return curr.Key == key && !curr.Marked;
        }
    }
}