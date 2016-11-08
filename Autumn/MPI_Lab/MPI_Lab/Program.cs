using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using System.IO;
using System.Windows;


namespace MPI_Lab
{
    [Serializable()]
    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                var world = Communicator.world;                 
                var vertsOutTree = new List<int>(); //вершины, еще не добавленные в наш граф
                var vertsInTree = new List<int>(); //вершины, уже добавленные в наш граф
                var edgesInTree = new List<Pair<int, int>>(); //ребра, добавленные в наш граф 
                if (world.Rank == 0)
                {
                    StreamReader tstfile;
                    Console.WriteLine("Infile:");
                    string fileName = Console.ReadLine();
                    try
                    {
                        tstfile = new StreamReader(fileName);
                    }
                    catch
                    {
                        Console.WriteLine("I need file!");
                        for (int i = 1; i < world.Size; i++)
                        {
                            world.Send<bool>(false, i, 0);
                        }
                        return;
                    }
                    string line = tstfile.ReadLine();
                    int n = Convert.ToInt32(line);
                    int[,] matrix = new int[n, n];
                    while ((line = tstfile.ReadLine()) != null) //запись информации из файла
                    {
                        int v1 = Convert.ToInt32(line.Split(' ')[0]);
                        int v2 = Convert.ToInt32(line.Split(' ')[1]);
                        int value = Convert.ToInt32(line.Split(' ')[2]);
                        matrix[v1, v2] = matrix[v2, v1] = value;
                    }
                    tstfile.Close();
                    for (int i = 1; i < world.Size; i++) //рассылка общей информации
                    {
                        world.Send<bool>(true, i, 0);
                        world.Send<int>(n, i, 1);
                        world.Send<int[,]>(matrix, i, 2);
                    }

                    vertsInTree.Add(0);
                    for (int i = 1; i < n; i++)
                    {
                        vertsOutTree.Add(i);
                    }

                    while (vertsOutTree.Count != 0)
                    {
                        for (int i = 1; i < world.Size; i++)
                        {
                            world.Send<bool>(true, i, 3);
                        }
                        int min = 0;
                        Pair<int, int> resEdge = new Pair<int, int>();
                        for (int i = 1; i < world.Size; i++)
                        {
                            Pair<int,int> curEdge = world.Receive<Pair<int, int>>(i, 4);
                            if (curEdge.First>=0 && (matrix[curEdge.First, curEdge.Second]<min || min == 0))
                            {
                                min = matrix[curEdge.First, curEdge.Second];
                                resEdge = curEdge;
                            }
                        }
                        edgesInTree.Add(resEdge);
                        vertsInTree.Add(resEdge.Second);
                        vertsOutTree.Remove(resEdge.Second);
                        for (int i = 1; i < world.Size; i++)
                        {
                            world.Send<int>(resEdge.Second, i, 5);
                        }
                    }
                    for (int i = 1; i < world.Size; i++)
                    {
                        world.Send<bool>(false, i, 3);
                    }

                    int res = 0;
                    foreach (Pair<int,int> edge in edgesInTree)
                    {
                        res += matrix[edge.First, edge.Second];
                    }
                    Console.WriteLine("Outfile:");
                    fileName = Console.ReadLine();
                    try
                    {
                        StreamWriter outfile = new StreamWriter(new FileStream(fileName, FileMode.Create));
                        outfile.WriteLine(n.ToString());
                        outfile.WriteLine(res.ToString());
                        outfile.Close();
                        Console.WriteLine("All is good!");
                    }
                    catch
                    {
                        Console.WriteLine("Oh! Outfile is fail! But res is " + res.ToString());
                    }
                }
                else
                {
                    bool fail = !world.Receive<bool>(0, 0);
                    if (fail)
                        return;
                    int n = world.Receive<int>(0, 1);
                    int k = n / (world.Size-1); //количество вершин на процессоре (кроме последнего)
                    int firstVert = k * (world.Rank - 1); //первая вершина процессора
                    int lastVert; //последняя вершина процессора
                    if (world.Rank == world.Size - 1)
                        lastVert = n - 1; //на последнем еще и оставшиеся вершины
                    else
                        lastVert = k * world.Rank - 1;
                    List<int> vertsOnProc = new List<int>();
                    for (int i=0; i <= lastVert - firstVert; i++)
                    {
                        vertsOnProc.Add(firstVert + i);
                    }
                    vertsOnProc.Remove(0);
                    int[,] matrix = world.Receive<int[,]>(0, 2);
                    vertsInTree.Add(0);


                    while(world.Receive<bool>(0, 3))
                    {
                        int min = 0;
                        var resEdge = new Pair<int, int>();
                        resEdge.First = resEdge.Second = -1;
                        foreach (int vert1 in vertsInTree)
                        {
                            foreach (int vert2 in vertsOnProc)
                            {
                                int value = matrix[vert1, vert2];
                                if (value > 0 && (value < min || min == 0))
                                {
                                    min = value;
                                    resEdge.First = vert1;
                                    resEdge.Second = vert2;
                                }
                            }
                        }
                        world.Send<Pair<int, int>>(resEdge, 0, 4);
                        int resVert = world.Receive<int>(0, 5);
                        vertsInTree.Add(resVert);
                        vertsOnProc.Remove(resVert);
                    }
                }
          }
       }
    }
}
