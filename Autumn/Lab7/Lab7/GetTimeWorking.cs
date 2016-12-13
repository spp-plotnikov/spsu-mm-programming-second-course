using System.Collections.Generic;

namespace Lab7
{
    class GetTimeWorking
    {
        SimpleImplementation _firstImplementation;
        ImplementationWithCuckooHashing _secondImplementation;
        int _capacity;
        private List<KeyValuePair<long, long>> _students;

        public GetTimeWorking(int capacity, List<KeyValuePair<long, long>> students)
        {
            _capacity = capacity;
            _students = students;
            _firstImplementation = new SimpleImplementation();
            _secondImplementation = new ImplementationWithCuckooHashing();
        }

        public long FirstType()
        {
            int block = _capacity / 100;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < block * 9; i++)
            {
                _firstImplementation.Add(_students[i].Key, _students[i].Value);
            }

            for (int i = 0; i < block * 90; i++)
            {
                _firstImplementation.Contains(_students[i].Key, _students[i].Value);
            }

            for (int i = 0; i < block; i++)
            {
                _firstImplementation.Remove(_students[i].Key, _students[i].Value);
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long SecondType()
        {
            int block = _capacity / 100;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < block * 9; i++)
            {
                _secondImplementation.Add(_students[i].Key, _students[i].Value);
            }

            for (int i = 0; i < block * 90; i++)
            {
                _secondImplementation.Contains(_students[i].Key, _students[i].Value);
            }

            for (int i = 0; i < block; i++)
            {
                _secondImplementation.Remove(_students[i].Key, _students[i].Value);
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
