using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Floyd
{
    public class Algorithm
    {
        private static int numP; //number of process
        private static int curRank; //rank of the current process
        private static int numV; //number of vertices
        private static int inf = 200000000; //the largest possible value
        private static int[] counts; //numbers of rows for every process
        private static int[] rowsVsP;//the last row in each process

        private static int Min(int x, int y)
        {
            return (x < y) ? x : y;
        }

        private static int[] ParseMatrix(string fileName)
        {
            string line;
            int i, j, w;
            System.IO.StreamReader file = new System.IO.StreamReader(@fileName);
            numV = Int32.Parse(file.ReadLine()); //get the number of vertices
            int[] adjMatrix = new int[numV * numV];
            for (i = 0; i < adjMatrix.Count(); i++)
            {
                adjMatrix[i] = inf;
            }
            //initialize the adjacency matrix
            while ((line = file.ReadLine()) != null)
            {
                String[] st = line.Split(' ');
                i = Int32.Parse(st[0]);
                j = Int32.Parse(st[1]);
                w = Int32.Parse(st[2]);
                adjMatrix[i * numV + j] = w;
            }
            return adjMatrix;
        }


        private static void SaveArrayToFile(int[] answer, String fileName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@fileName);
            for (int i = 0; i < numV; i++)
            {
                for (int j = 0; j < numV; j++)
                {
                    file.Write(answer[i*numV + j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
        }



        private static void FloydTask(int[] curTape)
        {
            int[] kRow = new int[numV];          
            for (int k = 0; k < numV; k++)
            {
                int proc = 0;
                //get the number of proccess that counts k-row
                for (int pr = 1; pr < numP; ++pr)
                {
                    if (k <= rowsVsP[pr])
                    {
                        if (k > rowsVsP[pr - 1])
                        {
                            proc = pr;
                        }
                    }
                }

                if (proc == curRank)
                {
                    for (int z = 0; z < numV; z++)
                    {
                        kRow[z] = curTape[(numV * (k % counts[proc])) + z];
                    }
                }

                MPI.Communicator.world.Broadcast(ref kRow,proc);
                for (int i = 0; i < counts[curRank]; i++)
                {
                    for (int j = 0; j < numV; j++)
                    {
                        curTape[i * numV + j] = Min(curTape[i * numV + j], (curTape[i * numV + k] + kRow[j]) % inf);
                    }

                }
            }
        }
       

        public static void Main(string[] args)
        {
            int[] adjMatrix = ParseMatrix("graph1000.txt");
            int[] answMatrix;
            using (new MPI.Environment(ref args))
            {
                numP = MPI.Communicator.world.Size;
                counts = new int[numP];
                rowsVsP = new int[numP];
                int freeRows = numV;
                int tapeH = numV / numP; //the height of every tape (tape - rows of current process)
                curRank = MPI.Communicator.world.Rank;

               //distribute matrix among the processes
                for (int i = 0; i < numP-1; i++)
                {
                    counts[i] = tapeH;
                    freeRows -= tapeH; 
                    tapeH = freeRows / (numP-i-1);
                }
                counts[numP-1] = freeRows;

                int[] sizes = new int[numP]; //the number of vertices in every process
                int count = 0;
                for (int i = 0; i < numP; i++)
                {
                    sizes[i] = counts[i] * numV;
                    rowsVsP[i] = count + counts[i]-1;
                    count += counts[i];
                }
                int[] curTape = new int[sizes[curRank]];
                MPI.Intracommunicator.world.ScatterFromFlattened(adjMatrix, sizes, 0, ref curTape);
                FloydTask(curTape);
                answMatrix = new int[numV * numV];
                MPI.Intracommunicator.world.GatherFlattened(curTape, sizes, 0, ref answMatrix);
                if (curRank ==0)
                {
                   SaveArrayToFile(answMatrix, "answer.txt");
                   Console.ReadLine();
                }
               
            }
        }
    }
}

