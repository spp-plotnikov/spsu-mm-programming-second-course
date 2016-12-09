using System;
using System.IO;

class AdjacencyMatrix
{
    private int[] matrix;
    private int size;

    private const int inf = 100000000;

    public AdjacencyMatrix(int[] matrix, int size)
    {
        this.size = size;
        this.matrix = new int[size * size];        
        this.matrix = matrix;
    }

    public AdjacencyMatrix(string path)
    {
        StreamReader fileHandler = new StreamReader(path);

        this.size = Int32.Parse(fileHandler.ReadLine());

        this.matrix = new int[this.size * this.size];

        this.Init(inf, 0);

        string line;
        while ((line = fileHandler.ReadLine()) != null)
        {
            string[] param = line.Split(' ');
            int i = Int32.Parse(param[0]);
            int j = Int32.Parse(param[1]);
            int w = Int32.Parse(param[2]);
            this.matrix[i * this.size + j] = w;
            this.matrix[j * this.size + i] = w;
        }
        fileHandler.Close();
    }

    private void Init(int cell, int diag)
    {
        for (int i = 0; i < this.size * this.size; i++)
            this.matrix[i] = cell;        
        for (int i = 0; i < this.size; i++)
            this.matrix[i * this.size + i] = diag;
    }

    public int Size
    {
        get 
        {
            return this.size;
        }
    }

    public int[] GetMatrix()
    {
        return this.matrix;
    }

    public void Print(string path = "")
    {
        if (path != "")
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    file.Write(this.matrix[i * this.size + j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
            return;
        }
        for (int i = 0; i < this.size; ++i)
        {
            for (int j = 0; j < this.size; ++j)
                Console.Write(this.matrix[i * this.size + j] + " ");
            Console.WriteLine();
        }
    }
}

