using System;
using System.IO;

class Matrix
{
    public int[] matrix;
    public int Size;

    public const int INF = 100000000;

    public Matrix(int[] matrix, int Size)
    {
        this.Size = Size;
        this.matrix = new int[Size * Size];        
        this.matrix = matrix;
    }

    public Matrix(string path)
    {
        StreamReader fileHandler = new StreamReader(path);

        this.Size = Int32.Parse(fileHandler.ReadLine());

        this.matrix = new int[this.Size * this.Size];

        this.init(INF, 0);

        string line;
        while ((line = fileHandler.ReadLine()) != null)
        {
            string[] param = line.Split(' ');
            int i = Int32.Parse(param[0]);
            int j = Int32.Parse(param[1]);
            int w = Int32.Parse(param[2]);
            this.matrix[i * this.Size + j] = w;
            this.matrix[j * this.Size + i] = w;
        }
        fileHandler.Close();
    }

    private void init(int cell, int diag)
    {
        for (int i = 0; i < this.Size * this.Size; i++)
            this.matrix[i] = cell;        
        for (int i = 0; i < this.Size; i++)
            this.matrix[i * this.Size + i] = diag;
    }

    public void print(string path = "")
    {
        if (path != "")
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    file.Write(this.matrix[i * this.Size + j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
            return;
        }
        for (int i = 0; i < this.Size; ++i)
        {
            for (int j = 0; j < this.Size; ++j)
                Console.Write(this.matrix[i * this.Size + j] + " ");
            Console.WriteLine();
        }
    }
}

