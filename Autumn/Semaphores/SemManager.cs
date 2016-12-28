using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semaphores
{
    public  class SemManager : IDisposable
    {
        private  bool[] _flagP;
        private bool[]  _flagC;

        List<int> buf = new List<int>();

        private  int _qProd = 1;
        private  int _qCons = 3;

        private   List<Thread> _pList = new List<Thread>(); // storage
        private  List<Thread> _cList = new List<Thread>();

        public  Semaphore Lock = new Semaphore(1, 1);

        public  void Dispose ()
        {
            StopC();
            StopP();

            Thread.Sleep(2000);
            
            /*Clearing */
            for (int i = 0; i < _qCons; i++)           
              _cList[0].Join();
           
            for (int i = 0; i < _qProd; i++)
                _pList[0].Join();
        }

        public void StartP(int numProd)
        {
            _qProd = numProd;
            Array.Resize<bool>(ref _flagP, _qProd);

            for (int j = 0; j < _qProd; j++)
            {
                int index = j;
                Producer tmp = new Producer(j);
                Thread prod = new Thread(() => tmp.Run(ref _flagP[index], ref Lock, ref buf));
                _flagP[j] = true;
                _pList.Add(prod);
                prod.Start();
            }

        }

        public void StopP()
        {
            for (int i = 0; i < _qProd; i++)
                _flagP[i] = false;
        }


        public void StartC(int numCons)
        {
            _qCons = numCons;
            Array.Resize<bool>(ref _flagC, _qCons);
            for (int i = 0; i < _qCons; i++)
            {
                int index = i;
                Consumer consumerObj = new Consumer(i);
                Thread cons = new Thread(() => consumerObj.Run(ref _flagC[index], ref Lock, ref buf));
                _flagC[i] = true;
                _cList.Add(cons);
                cons.Start();
            }

        }

        public void StopC()
        {
            for (int i = 0; i < _qCons; i++)
                _flagC[i] = false;
        }

    }
}
