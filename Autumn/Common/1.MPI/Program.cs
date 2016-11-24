using System;
using System.IO;
using System.Collections.Generic;
using MPI;
using System.Diagnostics;

class MPIHello
{
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

            // <master> context
            if (Communicator.world.Rank == 0)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Matrix matrix = new Matrix(inputFilePath);
                //matrix.print();
                
                int procNum = (comm.Size - 1);
 
                int tasksNum = (matrix.Size / procNum);
                int tasksOver = (matrix.Size % procNum);

                // distribution: block size for each process 
                int[] blockSize = new int[procNum + 1];

                // sending config to each process
                for (int p = 1; p <= procNum; ++p)
                {
                    TaskConfig conf;
                    if (p > matrix.Size)
                    {
                        blockSize[p] = -1;
                        conf = new TaskConfig(false);
                    }
                    else
                    {
                        if (tasksNum == 0)
                            blockSize[p] = 1;
                        else
                            blockSize[p] = (p == Math.Min(procNum, matrix.Size)) ? (tasksNum + tasksOver) : tasksNum;
                        conf = new TaskConfig(matrix.Size, blockSize[p]);
                    }
                        
                    comm.Send<TaskConfig>(conf, p, 5);
                }


                // start Floyd 
                for (int k = 0; k < matrix.Size; ++k)
                {
                    int blockIterator = 0;
                    for (int proc = 1; proc <= procNum; ++proc)
                    { 
                        Request[] rows = new Request[blockSize[proc]];
                           
                        int rowItem = 0;
                        for (int i = blockIterator; i < blockIterator + blockSize[proc]; ++i)
                        {                           
                            Edge ik = new Edge(i, k, matrix.getCell(i, k));   
                            rows[rowItem] = new Request(ik, matrix.getRow(i));
                            rowItem++;
                        }
                        // sending k'th row (for kj edge) only once for each process    
                        BlockOfRows req = new BlockOfRows(rows, matrix.getRow(k));
                        comm.Send<BlockOfRows>(req, proc, 0);

                        blockIterator += blockSize[proc];
                    }

                    blockIterator = 0;
                    // recieving and updating 
                    for (int proc = 1; proc <= procNum; ++proc)
                    {
                        Edge[] resp = comm.Receive<Edge[]>(Communicator.anySource, 2);
                        for (int o = 0; o < resp.Length; ++o)
                            matrix.updateCell(resp[o].from, resp[o].to, resp[o].value);
                        blockIterator += blockSize[proc];
                    }

                }

                matrix.print(outputFilePath);
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);//*/
            }
            else // <worker> context
            {
                // getting config 
                TaskConfig config = comm.Receive<TaskConfig>(0, 5);
                
                // this check need when proc number > matrix size. excess processes will be "sleep" 
                if (config.Runnable)
                {
                    for (int k = 0; k < config.Size; ++k)
                    {
                        BlockOfRows req1 = comm.Receive<BlockOfRows>(0, 0);
                        Request[] req = new Request[req1.msg.Length];
                        req = req1.msg;

                        // list of updated edges
                        List<Edge> resp = new List<Edge>();
                        for (int i = 0; i < config.BlockSize; ++i)
                        {
                            for (int j = 0; j < config.Size; ++j)
                            {
                                if (req[i].ik.value + req1.row_k[j] < req[i].row_i[j])
                                {
                                     Edge updatedEdge = new Edge(req[i].ik.from, j, req[i].ik.value + req1.row_k[j]);
                                     resp.Add(updatedEdge);
                                 }
                             }
                        }
                        comm.Send<Edge[]>(resp.ToArray(), 0, 2);
                    }
                }
            }
        }
    }
}