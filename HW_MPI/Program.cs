using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MPI;

namespace QSort
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                 if (args.Count() != 2)
                  {
                      Console.WriteLine("Invalid number of input parameters. 2 filenames expected.");
                      return;
                  }


                string fileNameIn = args[0];
                string fileNameOut = args[1];
                Communicator comm = Communicator.world;

            
      
                if(comm.Rank == 0)
                {

                    System.IO.StreamReader fileIn = new System.IO.StreamReader(@fileNameIn);
                    string line = fileIn.ReadLine();
                    fileIn.Close();
                    String[] st = line.Split(' ');
                    int size = st.Length;
                    int[] array = new int[size];
                    for (int i = 0; i < size; i++)
                    {
                        array[i] = Int32.Parse(st[i]);
                    }
                    ZeroProc(array, fileNameOut);               
                }

                else
                {
                    NonZeroProc();
                }
                return;
            }
        }


        static void ZeroProc(int[] array, string fileNameOut)
        {
            int len = array.Length;
            int numOfProc = Communicator.world.Size;

            //some tricks for efficacy

            if (len < numOfProc || numOfProc == 1)
            {
                Array.Sort(array);
                for (int i = 1; i < numOfProc; i++)
                {
                    Communicator.world.Send(-1, i, 42);
                }
                System.IO.StreamWriter fileOutLocal = new System.IO.StreamWriter(@fileNameOut);
                for (int i = 0; i < len; i++)
                {
                    fileOutLocal.Write(array[i] + " ");
                }
                fileOutLocal.WriteLine();
                fileOutLocal.Close();
                return;
            }
            for (int i = 1; i < numOfProc; i++)
            {
                Communicator.world.Send(1, i, 42);
            }
            for (int i = 1; i < numOfProc; i++)
            {
                int mas = len / numOfProc + (len % numOfProc > i ? 1 : 0);
                Communicator.world.Send(mas, i, 0);
            }

            // split array

            List<int> partOfArray = new List<int>();
            for (int i = 0; i < numOfProc; i++)
            {
                int mas = len / numOfProc + (len % numOfProc > i ? 1 : 0);
                int[] toSend = new int[mas];
                for (int j = i; j < len; j += numOfProc)
                {
                    toSend[(j - i) / numOfProc] = array[j];
                }

                if(i != 0)
                {
                    Communicator.world.Send(mas, i, 0);
                    Communicator.world.Send(toSend, i, 1);
                }
                else
                {
                    for (int j = 0; j < mas; j++)
                    {
                        partOfArray.Add(toSend[j]);

                    }
                }        
            }

            //receive part of array
       
            partOfArray.Sort();
            List<int> keysElem = new List<int>();
            int numOfKeys = partOfArray.Count / numOfProc;
            for (int i = 0; i <= numOfKeys && i * numOfProc < partOfArray.Count; i++)
            {
                keysElem.Add(partOfArray[i * numOfProc]);
            }
            for (int i = 1; i < numOfProc; i++)
            {
                List<int> temp = new List<int>();
                Communicator.world.Receive(i, 2, out temp);
                for (int j = 0; j < temp.Count; j++)
                {
                   
                    keysElem.Add(temp[j]);
                }
            }
            List<int> idx = new List<int>();
            for (int i = 0; i < numOfProc - 1; i++)
            {
                int temp = keysElem.Count / numOfProc + (keysElem.Count % numOfProc > i ? 1 : 0);
                idx.Add(keysElem[temp]);
            }
            idx.Sort();

            //sending boundaries

            List<int> boundaries = new List<int>();
            for (int i = 0; i < numOfProc - 1; i++)
            {
                boundaries.Add(idx[i * (idx.Count / numOfProc)]);
            }
            for (int i = 1; i < numOfProc; i++)
            {
                Communicator.world.Send(boundaries, i, 2);
            }

            //split by bound

            int k = 0;
            List<List<int>> toMerge = new List<List<int>>();
            List<int> tmp = new List<int>();
            for (int i = 0; i < boundaries.Count; i++)
            {
                tmp = new List<int>();
                while (k < partOfArray.Count && partOfArray[k] <= boundaries[i])
                {
                    tmp.Add(partOfArray[k]);
                    k++;
                }
                if(i != Communicator.world.Rank)
                {
                    Communicator.world.Send(tmp, i, 4);
                }
                else if(tmp.Count != 0)
                {
                    toMerge.Add(tmp);
                }
            }
            while (k < partOfArray.Count)
            {
                tmp.Add(partOfArray[k]);
                k++;
            }
            if(Communicator.world.Size - 1 != Communicator.world.Rank)
            {
                Communicator.world.Send(tmp, Communicator.world.Size - 1, 4);
            }
            else if(tmp.Count != 0)
            {
                toMerge.Add(tmp);
            }

            //final part

            for (int i = 0; i < numOfProc; i++)
            {
                if(i != Communicator.world.Rank)
                {
                    tmp = new List<int>();
                    Communicator.world.Receive(i, 4, out tmp);
                    if(tmp.Count != 0) { 
                        toMerge.Add(tmp);
                    }
                }
            }
            List<int> ans = new List<int>();
            Merge(toMerge, ref ans);
            System.IO.StreamWriter fileOut = new System.IO.StreamWriter(@fileNameOut);
            for (int i = 0; i < ans.Count; i++)
            {
                fileOut.Write(ans[i] + " ");
            }
            for (int i = 1; i < numOfProc; i++)
            {
                tmp = new List<int>();
                Communicator.world.Receive(i, 5, out tmp);
                for (int j = 0; j < tmp.Count; j++)
                {
                    fileOut.Write(tmp[j] + " ");
                }
            }
     
            fileOut.WriteLine();
            fileOut.Close();
            return;
        }


        static void NonZeroProc()
        {
            int isAlive;
            Communicator.world.Receive(0, 42, out isAlive);
            if(isAlive == -1)
            {
                return;
            }

            //recevie part of array

            int len;
            Communicator.world.Receive(0, 0, out len);
            int[] partOfArray = new int[len];
            Communicator.world.Receive(0, 1, ref partOfArray);
            
            //sort of array

            Array.Sort(partOfArray);

            //send num of key elem

            int numOfProc = Communicator.world.Size;
            int numOfKeys = (partOfArray.Length - 1) / numOfProc;
            List<int> toSend = new List<int>();
            for (int i = 0; i <= numOfKeys; i++)
            {
                toSend.Add(partOfArray[i * numOfProc]);     
            }
            Communicator.world.Send(toSend, 0, 2);

            //recieving boundaries

            List<int> boundaries = new List<int>();
            Communicator.world.Receive(0, 2, out boundaries);
            int k = 0;
            List<List<int>> toMerge = new List<List<int>>();
            List<int> tmp = new List<int>();
            for (int i = 0; i < boundaries.Count; i++)
            {
                tmp = new List<int>();
                while (k < partOfArray.Length && partOfArray[k] <= boundaries[i])
                {
                    tmp.Add(partOfArray[k]);
                    k++;
                }
                if(i != Communicator.world.Rank)
                {
                    Communicator.world.Send(tmp, i, 4);
                }
                else if(tmp.Count != 0)
                {
                    toMerge.Add(tmp);
                }
            }
            while (k < partOfArray.Length)
            {
                tmp.Add(partOfArray[k]);
                k++;
            }
            if(Communicator.world.Size - 1 != Communicator.world.Rank)
            {
                Communicator.world.Send(tmp, Communicator.world.Size - 1, 4);
            }
            else if(tmp.Count != 0) 
            {
                toMerge.Add(tmp);
            }

            //final part

            for (int i = 0; i < numOfProc; i++)
            {
                if (i != Communicator.world.Rank)
                {
                    tmp = new List<int>();
                    Communicator.world.Receive(i, 4, out tmp);
                    if (tmp.Count != 0)
                    {
                        toMerge.Add(tmp);
                    }
                }
            }
            List<int> ans = new List<int>();
            Merge(toMerge, ref ans);
            Communicator.world.Send(ans, 0, 5);
            return;
        }


        static void Merge(List<List<int>> toMerge, ref List <int> res)
        {
            List<int> idx = new List<int>();
            for (int i = 0; i < toMerge.Count; i++)
            {
                idx.Add(0);
            }
            bool end = false;
            while (!end)
            {
                end = true;
                int min = 1000000000;
                int num = -1;
                for (int i = 0; i < idx.Count; i++)
                {
                    if(toMerge[i].Count > idx[i] && toMerge[i][idx[i]] < min)
                    {
                        min = toMerge[i][idx[i]];
                        num = i;
                        end = false;
                    }
                }
                if(!end)
                {
                    res.Add(toMerge[num][idx[num]]);
                    idx[num]++;
                }
            }
            return;
        }
    }

}

