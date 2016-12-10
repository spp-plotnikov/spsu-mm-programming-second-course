using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibers
{
    /// <summary>
    /// Class for sampling from a discrete probability distribution.
    /// </summary>
    /// <remarks> A. J. Walker method </remarks>
    class Alias
    {
        bool _isInitialised = false;
        private double _sum = 0;
        private double _newSum = 0;
        private Random _rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        private OrderedDictionary _table = new OrderedDictionary();

        public void Add(uint Id, double prob)
        {
            _sum += prob;
            _table.Add(Id, prob);
        }

        void GenerateTable()
        {         
            IDictionaryEnumerator enumerator = _table.GetEnumerator();
            //queue!
            Queue<double> val = new Queue<double>();
            Queue<double> key = new Queue<double>();
            while (enumerator.MoveNext())
            {
                key.Enqueue((uint)enumerator.Key);
                val.Enqueue(_isInitialised ? (double)enumerator.Value * _sum / _newSum : (double)enumerator.Value / _sum);               
            }

            foreach (uint i in key)
            {
                _table[i] = val.Dequeue();
            }

            _sum = _isInitialised ? _newSum : _sum ;

            if (!_isInitialised)
                _isInitialised = true;

        }

        public object  Get()
        {
            if (!_isInitialised)
                GenerateTable();
            double prob = _rnd.NextDouble();
            IDictionaryEnumerator enumerator = _table.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (prob > (double)enumerator.Value)
                    prob -= (double)enumerator.Value;

                else
                    break;
            }
            return enumerator.Key;
        }

       public  void Delete(uint Id)
        {
            _newSum = _sum - ((double)_table[Id] * _sum);
            _table.Remove(Id);
            GenerateTable();
        }
    }
}
