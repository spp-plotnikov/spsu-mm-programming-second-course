using System;
using System.Threading;
using MPI;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MPIProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var env = new MPI.Environment(ref args))
            {
                var world = Communicator.world;
                int p = world.Size;    //процессора
                int deg = DegreeTwo(p);
                int n=16;            //число элементов
                var list = new List<int>();//список со всеми нашими числами
                int N;                 //изначальное число элементов на один процессор
                int berEl = 0;           //bearing element
                var ByteNomber = new int[deg + 1];//двоичное представление номера процессора

                string inputFileName = args[5];
                string outputFileName = args[6];
                var inputFile = new StreamReader(@inputFileName);
                var outputFile = new StreamWriter(@outputFileName);
                string read = inputFile.ReadLine();
                var splitLine = read.Split(' ');
                inputFile.Close();
                n = splitLine.Count();
                int[] arr = new int[n];
                for (int i = 0; i < n; i++)
                    arr[i] = int.Parse(splitLine[i]);


                if (n == n / p * p)
                    N = n / p;
                else
                    N = n / p + 1;

                if (world.Rank == 0)
                {
                    #region            //Создание рандомного массива. Надо будет считать из файла через Stream.
                    //var rand = new Random();
                    //for (int i = 0; i < n; i++)
                    //{
                    //    arr[i] = rand.Next(-100, 100);
                    //    Console.Write("{0} ", arr[i]);
                    //}
                    //Console.WriteLine();
                    #endregion

                    #region           //Участок кода на случай, когда число процессоров не N-мерный гиперкуб
                    if (!testDegreeTwo(p))
                    {
                        Console.WriteLine("\nЧисло процессоров не степень двойки, запустится непараллельный алгоритм\n" +
                            "Результат:");
                        qs(ref arr, 0, n - 1);
                        for (int i = 0; i < n; i++)
                            Console.Write("{0} ", arr[i]);
                        Thread.Sleep(500);
                    }
                }
                if (!testDegreeTwo(p))
                    return;
                #endregion

                if (world.Rank == 0)
                {
                    for (int i = 0; i < p - 1; i++)                   //рассылка на р процессоров
                        for (int j = N * i; j < N * (i + 1); j++)     //участков массивов размером N
                            world.Send<int>(arr[j], i + 1, j - N * i + 2);

                    for (int i = 1; i <= p - 1; i++)      //Рассылка опорного элемента и кол-ва пересылаемых элементов
                    {
                        world.Send<int>(berEl, i, 0);
                        world.Send<int>(N, i, 1);
                    }

                    int[] b = new int[n];
                    int bLenght = n - N * (p - 1);
                    int bNewLenght = bLenght;
                    for (int i = N * (p - 1); i < n; i++)
                        b[i - N * (p - 1)] = arr[i];
                    qs(ref b, 0, n - N * (p - 1) - 1);
                    solveByteNomber(ref ByteNomber, world.Rank);   //считаем двоичное представление номера процессора

                    //At this moment we have: 1.ByteNomber[], 2.b[] и bLenght, 3.n, p, N, 4.deg
                    int ind;
                    int jumpToSecondProcessor = p;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;

                        if (i != 0)
                            berEl = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                        ind = findIndexBerEl(berEl, b, bLenght);
                        world.Send<int>(bLenght - ind, world.Rank + jumpToSecondProcessor, 1);
                        for (int j = ind; j < bLenght; j++)
                            world.Send<int>(b[j], world.Rank + jumpToSecondProcessor, j - ind + 2);
                        bLenght = ind;
                        bNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                        for (int j = 0; j < bNewLenght; j++)
                            b[bLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                        bLenght += bNewLenght;
                        qs(ref b, 0, bLenght - 1);
                    }
                    string s = "";

                    for (int i = 0; i < bLenght; i++)
                        s = s + " " + b[i].ToString("G");
                    outputFile.WriteLine("world.Rank=" + world.Rank + ", b[]: " + s);
                    if (world.Rank != p - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
                else
                {
                    int[] b = new int[n];
                    int bLenght = world.Receive<int>(0, 1);
                    int bNewLenght = bLenght;
                    berEl = world.Receive<int>(0, 0);
                    for (int j = 0; j < bLenght; j++)
                        b[j] = world.Receive<int>(0, j + 2);
                    qs(ref b, 0, bLenght - 1);
                    solveByteNomber(ref ByteNomber, world.Rank);   //считаем двоичное представление номера процессора
                    int ind;
                    int jumpToSecondProcessor = p;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;
                        if (ByteNomber[deg - i] == 1)
                        {
                            if (i != 0)
                            {
                                berEl = b[bLenght / 2];
                                world.Send<int>(berEl, world.Rank - jumpToSecondProcessor, 0);
                            }
                            ind = findIndexBerEl(berEl, b, bLenght);
                            world.Send<int>(ind, world.Rank - jumpToSecondProcessor, 1);
                            for (int j = ind; j > 0; j--)
                                world.Send<int>(b[ind - j], world.Rank - jumpToSecondProcessor, ind - j + 2);
                            for (int j = ind; j < bLenght; j++)
                                b[j - ind] = b[j];
                            bLenght -= ind;
                            bNewLenght = world.Receive<int>(world.Rank - jumpToSecondProcessor, 1);
                            for (int j = 0; j < bNewLenght; j++)
                                b[bLenght + j] = world.Receive<int>(world.Rank - jumpToSecondProcessor, j + 2);
                            bLenght += bNewLenght;
                            qs(ref b, 0, bLenght - 1);
                        }
                        else
                        {
                            if (i != 0)
                                berEl = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                            ind = findIndexBerEl(berEl, b, bLenght);
                            world.Send<int>(bLenght - ind, world.Rank + jumpToSecondProcessor, 1);
                            for (int j = ind; j < bLenght; j++)
                                world.Send<int>(b[j], world.Rank + jumpToSecondProcessor, j - ind + 2);
                            bLenght = ind;
                            bNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                            for (int j = 0; j < bNewLenght; j++)
                                b[bLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                            bLenght += bNewLenght;
                            qs(ref b, 0, bLenght - 1);
                        }
                    }
                    string s = "";
                    world.Receive<int>(world.Rank - 1, 0);
                    
                    for (int i = 0; i < bLenght; i++)
                        s = s + " " + b[i].ToString("G");
                    outputFile.WriteLine("world.Rank=" + world.Rank + ", b[]: " + s);
                    if (world.Rank != p - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
                outputFile.Close();
            }

            Thread.Sleep(50000);
        }

        static void solveByteNomber(ref int[] ByteNomber, int processor)
        {
            int i= 1;
            while(processor>0)
            {
                ByteNomber[i] = processor % 2;
                processor /= 2;
                i++;
            }

        }

        static void qs(ref int[] arr, int l, int r)
        {
            if (l < r)
            {
                int m = l;
                int c = r+1;
                int e = 1;
                while (c != m)
                {
                    c = c - e;
                    while (e * arr[m] < e * arr[c])
                        c = c - e;
                    int t = arr[m];
                    arr[m] = arr[c];
                    arr[c] = t;
                    t = m;
                    m = c;
                    c = t;
                    e = -e;
                }
                qs(ref arr, l, m - 1);
                qs(ref arr, m + 1, r);
            }
        }
        
        static bool testDegreeTwo(int p)
        {
            int _k = 1;
            int _p = p;
            while (_p > 1)
            {
                _p = _p / 2;
                _k = _k * 2;
            }
            if (p == _k)
                return true;
            else
                return false;
        }

        static int DegreeTwo(int p)
        {
            if (testDegreeTwo(p))
            {
                int _k = 0;
                int _p = p;
                while (_p > 1)
                {
                    _p = _p / 2;
                    _k++;
                }
                return _k;
            }
            else
            {
                Console.WriteLine("Не степень двойки");
                return -1;
            }

        }
        
        static int findIndexBerEl(int berEl, int[] b, int bLength)       //Ответ - кол-во меньших чем berEl
        {                                                               
            int ind=0;
            int first=0;
            int last= bLength;// last=4;
            while (first < last)
            {
                ind = first + (last - first) / 2;
                //berEl=0
                if (berEl <= b[ind])             // все отрицательные -4 -3 -2 -1
                    last = ind;                 //все положительные  1 2 3 4
                else                            //и те и те.  -2 -1 1 2
                {                            
                    first = ind + 1;
                    if (first == last)
                        ind++;
                }
            }
            return ind;
        }
    }
}