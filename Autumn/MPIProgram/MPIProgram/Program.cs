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
                int proc = world.Size;             //процессора
                int deg = ConsiderDegreeTwo(proc);
                int numberOfElement;            //число элементов
                int primaryNumberOnEachProc;    //изначальное число элементов на один процессор
                int bearingElem = 0;                
                var ByteNumber = new int[deg + 1]; //двоичное представление номера процессора

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

                if (numberOfElement == numberOfElement / proc * proc)
                    primaryNumberOnEachProc = numberOfElement / proc;
                else
                    primaryNumberOnEachProc = numberOfElement / proc + 1;

                #region           //Участок кода на случай, когда число процессоров не N-мерный гиперкуб
                if (world.Rank == 0)
                {
                    if (!TestDegreeTwo(proc))
                    {
                        Console.WriteLine("\nЧисло процессоров не степень двойки, запустится непараллельный алгоритм\n" +
                            "Результат:");
                        QuickSort(ref arr, 0, numberOfElement - 1);
                        for (int i = 0; i < numberOfElement; i++)
                            Console.Write("{0} ", arr[i]);
                        Thread.Sleep(500);
                    }
                }
                if (!TestDegreeTwo(proc))
                    return;
                #endregion

                if (world.Rank == 0)
                {

                    for (int i = 0; i < proc - 1; i++)                   //рассылка на р процессоров
                        for (int j = primaryNumberOnEachProc * i; j < primaryNumberOnEachProc * (i + 1); j++)     //участков массивов размером N
                            world.Send<int>(arr[j], i + 1, j - primaryNumberOnEachProc * i + 2);
                    
                    int[] otherArr = new int[numberOfElement];
                    int otherArrLenght = numberOfElement - primaryNumberOnEachProc * (proc - 1);
                    int otherArrNewLenght = otherArrLenght;
                    for (int i = primaryNumberOnEachProc * (proc - 1); i < numberOfElement; i++)
                        otherArr[i - primaryNumberOnEachProc * (proc - 1)] = arr[i];
                    QuickSort(ref otherArr, 0, numberOfElement - primaryNumberOnEachProc * (proc - 1) - 1);
                    bearingElem = otherArr[(numberOfElement - primaryNumberOnEachProc * (proc - 1)) / 2];
                    for (int i = 1; i <= proc - 1; i++)      //Рассылка опорного элемента и кол-ва пересылаемых элементов
                    {
                        world.Send<int>(bearingElem, i, 0);
                        world.Send<int>(primaryNumberOnEachProc, i, 1);
                    }
                    SolveByteNomber(ref ByteNumber, world.Rank);   //считаем двоичное представление номера процессора

                    //At this moment we have: 1.ByteNomber[], 2.b[] и bLenght, 3.n, p, N, 4.deg
                    int index;
                    int jumpToSecondProcessor = proc;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;

                        if (i != 0)
                            bearingElem = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                        index = FindIndexBerEl(bearingElem, otherArr, otherArrLenght);
                        world.Send<int>(otherArrLenght - index, world.Rank + jumpToSecondProcessor, 1);
                        for (int j = index; j < otherArrLenght; j++)
                            world.Send<int>(otherArr[j], world.Rank + jumpToSecondProcessor, j - index + 2);
                        otherArrLenght = index;
                        otherArrNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                        for (int j = 0; j < otherArrNewLenght; j++)
                            otherArr[otherArrLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                        otherArrLenght += otherArrNewLenght;
                        QuickSort(ref otherArr, 0, otherArrLenght - 1);
                    }

                    var outputFile = new StreamWriter(@outputFileName, false);
                    outputFile.Write("world.Rank=" + world.Rank + ", b[]: ");
                    for (int i = 0; i < otherArrLenght; i++)
                        outputFile.Write(otherArr[i] + " ");

                    outputFile.Close();

                    if (world.Rank != proc - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
                else
                {
                    int[] otherArr = new int[numberOfElement];
                    int otherArrLenght = world.Receive<int>(0, 1);
                    int otherArrNewLenght = otherArrLenght;
                    bearingElem = world.Receive<int>(0, 0);
                    for (int j = 0; j < otherArrLenght; j++)
                        otherArr[j] = world.Receive<int>(0, j + 2);
                    QuickSort(ref otherArr, 0, otherArrLenght - 1);
                    SolveByteNomber(ref ByteNumber, world.Rank);   //считаем двоичное представление номера процессора

                    int index;
                    int jumpToSecondProcessor = proc;
                    for (int i = 0; i < deg; i++)
                    {
                        jumpToSecondProcessor /= 2;
                        if (ByteNumber[deg - i] == 1)
                        {
                            if (i != 0)
                            {
                                bearingElem = otherArr[otherArrLenght / 2];
                                world.Send<int>(bearingElem, world.Rank - jumpToSecondProcessor, 0);
                            }
                            index = FindIndexBerEl(bearingElem, otherArr, otherArrLenght);
                            world.Send<int>(index, world.Rank - jumpToSecondProcessor, 1);
                            for (int j = index; j > 0; j--)
                                world.Send<int>(otherArr[index - j], world.Rank - jumpToSecondProcessor, index - j + 2);
                            for (int j = index; j < otherArrLenght; j++)
                                otherArr[j - index] = otherArr[j];
                            otherArrLenght -= index;
                            otherArrNewLenght = world.Receive<int>(world.Rank - jumpToSecondProcessor, 1);
                            for (int j = 0; j < otherArrNewLenght; j++)
                                otherArr[otherArrLenght + j] = world.Receive<int>(world.Rank - jumpToSecondProcessor, j + 2);
                            otherArrLenght += otherArrNewLenght;
                            QuickSort(ref otherArr, 0, otherArrLenght - 1);
                        }
                        else
                        {
                            if (i != 0)
                                bearingElem = world.Receive<int>(world.Rank + jumpToSecondProcessor, 0);
                            index = FindIndexBerEl(bearingElem, otherArr, otherArrLenght);
                            world.Send<int>(otherArrLenght - index, world.Rank + jumpToSecondProcessor, 1);
                            for (int j = index; j < otherArrLenght; j++)
                                world.Send<int>(otherArr[j], world.Rank + jumpToSecondProcessor, j - index + 2);
                            otherArrLenght = index;
                            otherArrNewLenght = world.Receive<int>(world.Rank + jumpToSecondProcessor, 1);
                            for (int j = 0; j < otherArrNewLenght; j++)
                                otherArr[otherArrLenght + j] = world.Receive<int>(world.Rank + jumpToSecondProcessor, j + 2);
                            otherArrLenght += otherArrNewLenght;
                            QuickSort(ref otherArr, 0, otherArrLenght - 1);
                        }
                    }
                    world.Receive<int>(world.Rank - 1, 0);

                    var outputFile = new StreamWriter(@outputFileName, true);
                    outputFile.Write("world.Rank=" + world.Rank + ", b[]: ");
                    for (int i = 0; i < otherArrLenght; i++)
                        outputFile.Write(otherArr[i] + " ");

                    outputFile.Close();

                    if (world.Rank != proc - 1)
                        world.Send<int>(0, world.Rank + 1, 0);
                }
            }
        }

        /// <summary>
        /// Represent int value to byte view
        /// </summary>
        /// <param name="ByteNomber">byte view</param>
        /// <param name="value">int value</param>
        static void SolveByteNomber(ref int[] ByteNomber, int value)
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
        static void QuickSort(ref int[] arr, int l, int r)
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
                QuickSort(ref arr, l, m - 1);
                QuickSort(ref arr, m + 1, r);
            }
        }
        
        /// <summary>
        /// Test the number to equal a degree two.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Bool answer</returns>
        static bool TestDegreeTwo(int p)
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
        static int ConsiderDegreeTwo(int p)
        {
            if (TestDegreeTwo(p))
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
                Console.WriteLine("Не степень двойки.");
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
        static int FindIndexBerEl(int bearEl, int[] b, int bLength)       //Ответ - кол-во меньших чем berEl
        {                                                               
            int ind=0;
            int first=0;
            int last= bLength; // last=4;
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