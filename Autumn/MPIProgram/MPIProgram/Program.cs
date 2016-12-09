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
                int p = world.Size;             //процессора
                int deg = DegreeTwo(p);
                int numberOfElement;            //число элементов
                int primaryNumberOnEachProc;    //изначальное число элементов на один процессор
                int bearEl = 0;                 //bearing element
                var ByteNomber = new int[deg + 1]; //двоичное представление номера процессора

                string inputFileName = args[0];
                string outputFileName = args[1];
                var inputFile = new StreamReader(@inputFileName);
                string read = inputFile.ReadLine();
                var splitLine = read.Split(' ');
                inputFile.Close();
                numberOfElement = splitLine.Count();
                int[] arr = new int[numberOfElement+1];
                if (world.Rank == 0)
                    for (int i = 0; i < numberOfElement; i++)
                        arr[i] = int.Parse(splitLine[i]);

                if (numberOfElement == numberOfElement / p * p)
                    primaryNumberOnEachProc = numberOfElement / p;
                else
                    primaryNumberOnEachProc = numberOfElement / p + 1;

                #region           //Участок кода на случай, когда число процессоров не N-мерный гиперкуб
                if (world.Rank == 0)
                {
                    if (!testDegreeTwo(p))
                    {
                        Console.WriteLine("\nЧисло процессоров не степень двойки, запустится непараллельный алгоритм\n" +
                            "Результат:");
                        qs(ref arr, 0, numberOfElement - 1);
                        for (int i = 0; i < numberOfElement; i++)
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
                        for (int j = primaryNumberOnEachProc * i; j < primaryNumberOnEachProc * (i + 1); j++)     //участков массивов размером N
                            world.Send<int>(arr[j], i + 1, j - primaryNumberOnEachProc * i + 2);
                    
                    int[] b = new int[numberOfElement];
                    int bLenght = numberOfElement - primaryNumberOnEachProc * (p - 1);
                    int bNewLenght = bLenght;
                    for (int i = primaryNumberOnEachProc * (p - 1); i < numberOfElement; i++)
                        b[i - primaryNumberOnEachProc * (p - 1)] = arr[i];
                    qs(ref b, 0, numberOfElement - primaryNumberOnEachProc * (p - 1) - 1);
                    bearEl = b[(numberOfElement - primaryNumberOnEachProc * (p - 1)) / 2];
                    for (int i = 1; i <= p - 1; i++)      //Рассылка опорного элемента и кол-ва пересылаемых элементов
                    {
                        world.Send<int>(bearEl, i, 0);
                        world.Send<int>(primaryNumberOnEachProc, i, 1);
                    }
                    solveByteNomber(ref ByteNomber, world.Rank);   //считаем двоичное представление номера процессора

                    //At this moment we have: 1.ByteNomber[], 2.b[] и bLenght, 3.n, p, N, 4.deg
                    int index;
                    int jumpToSecondProcessor = p;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;

                        if (i != 0)
                            bearEl = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                        index = findIndexBerEl(bearEl, b, bLenght);
                        world.Send<int>(bLenght - index, world.Rank + jumpToSecondProcessor, 1);
                        for (int j = index; j < bLenght; j++)
                            world.Send<int>(b[j], world.Rank + jumpToSecondProcessor, j - index + 2);
                        bLenght = index;
                        bNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                        for (int j = 0; j < bNewLenght; j++)
                            b[bLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                        bLenght += bNewLenght;
                        qs(ref b, 0, bLenght - 1);
                    }
                    var outputFile = new StreamWriter(@outputFileName, false);
                    outputFile.Write("world.Rank=" + world.Rank + ", b[]: ");
                    for (int i = 0; i < bLenght; i++)
                        outputFile.Write(b[i] + " ");
                    outputFile.Close();
                    if (world.Rank != p - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
                else
                {
                    int[] b = new int[numberOfElement];
                    int bLenght = world.Receive<int>(0, 1);
                    int bNewLenght = bLenght;
                    bearEl = world.Receive<int>(0, 0);
                    for (int j = 0; j < bLenght; j++)
                        b[j] = world.Receive<int>(0, j + 2);
                    qs(ref b, 0, bLenght - 1);
                    solveByteNomber(ref ByteNomber, world.Rank);   //считаем двоичное представление номера процессора
                    int index;
                    int jumpToSecondProcessor = p;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;
                        if (ByteNomber[deg - i] == 1)
                        {
                            if (i != 0)
                            {
                                bearEl = b[bLenght / 2];
                                world.Send<int>(bearEl, world.Rank - jumpToSecondProcessor, 0);
                            }
                            index = findIndexBerEl(bearEl, b, bLenght);
                            world.Send<int>(index, world.Rank - jumpToSecondProcessor, 1);
                            for (int j = index; j > 0; j--)
                                world.Send<int>(b[index - j], world.Rank - jumpToSecondProcessor, index - j + 2);
                            for (int j = index; j < bLenght; j++)
                                b[j - index] = b[j];
                            bLenght -= index;
                            bNewLenght = world.Receive<int>(world.Rank - jumpToSecondProcessor, 1);
                            for (int j = 0; j < bNewLenght; j++)
                                b[bLenght + j] = world.Receive<int>(world.Rank - jumpToSecondProcessor, j + 2);
                            bLenght += bNewLenght;
                            qs(ref b, 0, bLenght - 1);
                        }
                        else
                        {
                            if (i != 0)
                                bearEl = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                            index = findIndexBerEl(bearEl, b, bLenght);
                            world.Send<int>(bLenght - index, world.Rank + jumpToSecondProcessor, 1);
                            for (int j = index; j < bLenght; j++)
                                world.Send<int>(b[j], world.Rank + jumpToSecondProcessor, j - index + 2);
                            bLenght = index;
                            bNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                            for (int j = 0; j < bNewLenght; j++)
                                b[bLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                            bLenght += bNewLenght;
                            qs(ref b, 0, bLenght - 1);
                        }
                    }
                    world.Receive<int>(world.Rank - 1, 0);
                    var outputFile = new StreamWriter(@outputFileName, true);
                    outputFile.Write("world.Rank=" + world.Rank + ", b[]: ");
                    for (int i = 0; i < bLenght; i++)
                        outputFile.Write(b[i] + " ");
                    outputFile.Close();
                    if (world.Rank != p - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
            }
        }

        /// <summary>
        /// Represent int value to byte view
        /// </summary>
        /// <param name="ByteNomber">byte view</param>
        /// <param name="value">int value</param>
        static void solveByteNomber(ref int[] ByteNomber, int value)
        {
            int i= 1;
            while(value>0)
            {
                ByteNomber[i] = value % 2;
                value /= 2;
                i++;
            }
        }

        /// <summary>
        /// The method of sorting an array using quick sort algorithm.
        /// </summary>
        /// <param name="arr">sorted array</param>
        /// <param name="l">left element of array</param>
        /// <param name="r">right element of array</param>
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
        
        /// <summary>
        /// Test the number to equal a degree two.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Bool answer</returns>
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



        /// <summary>
        /// Consider degree of two.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>degree</returns>
        static int DegreeTwo(int p)
        {
            if (testDegreeTwo(p))
            {
                int _k = 0;
                while (p > 1)
                {
                    p = p / 2;
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

        /// <summary>
        /// Find index of the bearing element
        /// </summary>
        /// <param name="bearEl">bearing element</param>
        /// <param name="b"> array of value</param>
        /// <param name="bLength"> length array</param>
        /// <returns>index baerEl</returns>
        static int findIndexBerEl(int bearEl, int[] b, int bLength)       //Ответ - кол-во меньших чем berEl
        {                                                               
            int ind=0;
            int first=0;
            int last= bLength;// last=4;
            while (first < last)
            {
                ind = first + (last - first) / 2;
                //berEl=0
                if (bearEl <= b[ind])             // все отрицательные -4 -3 -2 -1
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