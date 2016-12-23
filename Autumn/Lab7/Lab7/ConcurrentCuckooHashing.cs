﻿using System.Collections.Generic;
using System.Threading;

namespace Lab7
{
    class ConcurrentCuckooHashing : CuckooHashing
    {
        readonly Mutex[] _firstLocker;
        readonly Mutex[] _secondLocker;
        long _capacity;
        int _numberOfLocker;

        public ConcurrentCuckooHashing(int capacity) : base(capacity)
        {
            _capacity = capacity;
            _numberOfLocker = 10;
            _firstLocker = new Mutex[_capacity];
            _secondLocker = new Mutex[_capacity];

            for (int i = 0; i < _capacity; i++)
            {
                _firstLocker[i] = new Mutex();
                _secondLocker[i] = new Mutex();
            }
        }

        public override void Acquire(KeyValuePair<long, long> x)
        {
            _firstLocker[FirstHash(x) % _numberOfLocker].WaitOne();
            _secondLocker[SecondHash(x) % _numberOfLocker].WaitOne();
        }

        public override void Release(KeyValuePair<long, long> x)
        {
            _firstLocker[FirstHash(x) % _numberOfLocker].ReleaseMutex();
            _secondLocker[SecondHash(x) % _numberOfLocker].ReleaseMutex();
        }

        public override void Acquire(List<KeyValuePair<long, long>> x)
        {
            Monitor.Enter(x);
        }

        public override void Release(List<KeyValuePair<long, long>> x)
        {
            Monitor.Exit(x);
        }

        public override void Resize()
        {
            foreach (var locker in _firstLocker)
            {
                locker.WaitOne();
            }
            try
            {
                long oldCapacity = _capacity;
                if (_capacity != oldCapacity)
                {
                    return;
                }
                List<KeyValuePair<long, long>>[,] oldTable = Table;
                _capacity = 2 * _capacity;
                Table = new List<KeyValuePair<long, long>>[2, _capacity];
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < _capacity; j++)
                    {
                        Table[i, j] = new List<KeyValuePair<long, long>>(ProbSize);
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < oldCapacity; j++)
                    {
                        foreach (var x in oldTable[i, j])
                        {
                            Table[0, FirstHash(x) % _capacity].Add(x);
                        }
                    }
                }
            }
            finally
            {
                foreach (var locker in _firstLocker)
                {
                    locker.ReleaseMutex();
                }
            }
        }
    }
}
