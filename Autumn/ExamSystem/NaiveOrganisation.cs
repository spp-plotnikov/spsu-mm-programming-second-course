using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class NaiveOrganisation : IExamSystem
    {
        List<StudInfo>[] _table = null;

        int _tsize = 10;
        int _lsize = 10;

        Mutex _mut = new Mutex();

        public NaiveOrganisation()
        { 
            _table = new List<StudInfo>[_tsize];

            for (int i = 0; i < _tsize; i++)
                _table[i] = new List<StudInfo>();
        }

        void Resize()
        {
            _mut.WaitOne();

            List<StudInfo>[] tmpBuf = _table;

            _tsize *= 10;
            _lsize *= 10;

            _table = new List<StudInfo>[_tsize];

            for (int i = 0; i < _tsize; i++)
                _table[i] = new List<StudInfo>();

            foreach (var list in tmpBuf)
                foreach (var i in list)
                    _table[i.Hash % _tsize].Add(i);

            _mut.ReleaseMutex();
        }

        public void Add(long studentID, long courceID)
        {
            StudInfo tmp = new StudInfo(studentID, courceID);
            int hash = tmp.Hash % _tsize;

            _mut.WaitOne();
            _table[hash].Add(tmp);               
            _mut.ReleaseMutex();

            if (_table[hash].Count > _lsize)
                Resize();

        }

        public bool Contains(long studentId, long courseId)
        {
            StudInfo tmp = new StudInfo(studentId, courseId);
            int hash = tmp.Hash % _tsize;

            _mut.WaitOne();
            foreach (var item in _table[hash])
                if (item.StudentID == tmp.StudentID)
                {
                    _mut.ReleaseMutex();
                    return true;
                }

            _mut.ReleaseMutex();
            return false;
        }

        public void Remove(long studentId, long courseId)
        {
            StudInfo tmp = new StudInfo(studentId, courseId);
            int hash = tmp.Hash % _tsize;
            _mut.WaitOne();
            int count = 0;
            foreach (var item in _table[hash])
                if (item.StudentID == tmp.StudentID)
                {
                    _table[hash].RemoveAt(count);
                    _mut.ReleaseMutex();
                    return;
                }
            
            _mut.ReleaseMutex();
        }
    }
}
