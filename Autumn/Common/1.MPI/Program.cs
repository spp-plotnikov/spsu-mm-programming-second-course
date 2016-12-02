using System;
using System.IO;
using MPI;

class MPIHello
{
    public static int[] blockRows;
    public static int[] blockEnd; 
           
    static void Floyd(int[] curBlock, int procNum, int procRank, int mSize)
    {   
        for (int k = 0; k < mSize; k++)
        {
            int[] kRow = new int[mSize]; // k'th row for kj 

            // get the proc that work with k'th row 
            int proc = 0;
            for (int p = 1; p < procNum; ++p)
                if ( blockEnd[p - 1] < k && k <= blockEnd[p])
                        proc = p;

            if (proc == procRank)
            {
                int rowNum = (proc > 0) ? k - blockEnd[proc - 1] - 1 : k;
                for (int j = 0; j < mSize; ++j)
                    kRow[j] = curBlock[mSize * rowNum + j];   
            }

            // sending k'th row to all processes for kj
            Communicator.world.Broadcast(ref kRow, proc);
            int curBlockEnd = blockRows[procRank] / mSize;

            // do Floid
            for (int i = 0; i < curBlockEnd; ++i)
                for (int j = 0; j < mSize; ++j)
                    if(curBlock[i * mSize + k] + kRow[j] < curBlock[i * mSize + j])
                        curBlock[i * mSize + j] = curBlock[i * mSize + k] + kRow[j];           
        }
    }


    static void Main(string[] args)
    {
        using (new MPI.Environment(ref args))
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid argument! Usage: <inFile> <outFile>");
                System.Environment.Exit(1);
            }
            string inputFilePath = args[0];
            string outputFilePath = args[1];

            // init communication interface
            Intracommunicator comm = Communicator.world;

            // open files and getting matrix
            ManageMatrix m = new ManageMatrix(inputFilePath);
             
            // process number 
            int procNum = comm.Size;

            int blocksNum = (m.GetSize() / procNum);
            int blocksOver = (m.GetSize() % procNum);

            // block rows for each process 
            blockRows = new int[procNum];
            // right bound of each block 
            blockEnd = new int[procNum];
               

            // distribution by blocks
            for (int p = 0; p < procNum; ++p)
            {
                int curBlockSize = (p == procNum - 1) ? (blocksNum + blocksOver) : blocksNum ;
                blockRows[p] = curBlockSize * m.GetSize(); // submatrix blocksize * n
                blockEnd[p] = (p > 0) ? curBlockSize + blockEnd[p - 1] : curBlockSize - 1;
            }
                

            // recived block
            int[] curBlock = new int[blockRows[comm.Rank]];
            // answer 
            int[] answMatrix = new int[m.GetSize() * m.GetSize()];

            // scattering to workers 
            comm.ScatterFromFlattened(m.GetMatrix(), blockRows, 0, ref curBlock);
            Floyd(curBlock, comm.Size, comm.Rank, m.GetSize());

            // gathering after Floyd
            comm.GatherFlattened(curBlock, blockRows, 0, ref answMatrix);
                
            // output 
            if (comm.Rank == 0)
            {
                ManageMatrix outMatrix = new ManageMatrix(answMatrix, m.GetSize());
                outMatrix.Print(outputFilePath);
                Console.WriteLine("Success!");
            }
        }
    }
}
