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
        enum tag
        {
            needToStart,
            sourceData,
            sortedData,
        };
        static void Main(string[] args)
        {
            if (args.Count() != 2)
            {
                Console.WriteLine("Args format: path_to_input path_to_output");
                Console.ReadKey();
                return;
            }

            using (new MPI.Environment(ref args))
            {
                var world = Communicator.world;
                if (world.Rank == 0)
                {
                    #region read data
                    StreamReader inputFile;
                    try
                    {
                        inputFile = new StreamReader(args[0]);
                    }
                    catch
                    {
                        Console.WriteLine("Can not open file");
                        for (int curProc = 1; curProc < world.Size; curProc++)
                        {
                            world.Send<bool>(false, curProc, (int)tag.needToStart);
                        }
                        return;
                    }

                    string unsplited = inputFile.ReadToEnd();
                    string[] spltd = unsplited.Split(' ');
                    int numOfElements = spltd.Count();
                    List<int> data = new List<int>();
                    for (int i = 0; i < numOfElements; i++)
                    {
                        data.Add(int.Parse(spltd[i]));
                    }

                    inputFile.Close();
                    #endregion

                    #region send data
                    int persDatCount = numOfElements / (world.Size - 1);
                    int additForLast = numOfElements % (world.Size - 1);
                    for (int curProc = 1; curProc <= world.Size - 2; curProc++)
                    {
                        List<int> personalData = new List<int>();
                        for (int k = (curProc - 1) * persDatCount; k < curProc * persDatCount; k++)
                        {
                            personalData.Add(data[k]);
                        }
                        world.Send<bool>(true, curProc, (int)tag.needToStart);
                        world.Send<List<int>>(personalData, curProc, (int)tag.sourceData);
                    }

                    List<int> dataForLast = new List<int>();
                    for (int k = (world.Size - 2) * persDatCount; k < numOfElements; k++)
                    {
                        dataForLast.Add(data[k]);
                    }

                    world.Send<bool>(true, world.Size - 1, (int)tag.needToStart);
                    world.Send<List<int>>(dataForLast, world.Size - 1, (int)tag.sourceData);
                    #endregion

                    #region get data & merge sort
                    List<int> sortedData = new List<int>();
                    for (int curProc = 1; curProc < world.Size; curProc++)
                    {
                        List<int> recivedData = world.Receive<List<int>>(curProc, (int)tag.sortedData);
                        int curPosition = 0;
                        for (int i = 0; i < recivedData.Count; i++)
                        {
                            if (sortedData.Count == 0)
                            {
                                sortedData.Add(recivedData[i]);
                                continue;
                            }
                            while (sortedData[curPosition] < recivedData[i] && curPosition < sortedData.Count - 1)
                            {
                                curPosition++;
                            }
                            sortedData.Insert(curPosition, recivedData[i]);
                        }
                    }
                    #endregion

                    #region print result
                    try
                    {
                        StreamWriter outfile = new StreamWriter(new FileStream(args[1], FileMode.Create));
                        for (int i = 0; i < numOfElements; i++)
                        {
                            outfile.Write(sortedData[i]);
                            outfile.Write(' ');
                        }
                        outfile.Close();
                        Console.WriteLine("Done");
                    }
                    catch
                    {
                        Console.WriteLine("Problem with output file\n");
                        for (int i = 0; i < numOfElements; i++)
                        {
                            Console.Write(sortedData[i] + ' ');
                        }
                    }
                    #endregion
                }

                else
                {
                    #region get & sort & send
                    if (world.Receive<bool>(0, (int)tag.needToStart) == false)
                        return;

                    List<int> sourceData = world.Receive<List<int>>(0, (int)tag.sourceData);

                    sourceData.Sort();

                    world.Send<List<int>>(sourceData, 0, (int)tag.sortedData);
                    #endregion
                }
            }
        }
    }
}
