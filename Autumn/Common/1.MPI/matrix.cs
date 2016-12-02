using System;
using System.IO;

class ManageMatrix
{
    private int[] Matrix;
    private int Size;

    private const int inf = 100000000;

    public ManageMatrix(int[] matrix, int size)
    {
        this.Size = size;
        this.Matrix = new int[size * size];        
        this.Matrix = matrix;
    }

    public ManageMatrix(string path)
    {
        StreamReader fileHandler = new StreamReader(path);

        this.Size = Int32.Parse(fileHandler.ReadLine());

        this.Matrix = new int[this.Size * this.Size];

        this.Init(inf, 0);

        string line;
        while ((line = fileHandler.ReadLine()) != null)
        {
            string[] param = line.Split(' ');
            int i = Int32.Parse(param[0]);
            int j = Int32.Parse(param[1]);
            int w = Int32.Parse(param[2]);
            this.Matrix[i * this.Size + j] = w;
            this.Matrix[j * this.Size + i] = w;
        }
        fileHandler.Close();
    }

    private void Init(int cell, int diag)
    {
        for (int i = 0; i < this.Size * this.Size; i++)
            this.Matrix[i] = cell;        
        for (int i = 0; i < this.Size; i++)
            this.Matrix[i * this.Size + i] = diag;
    }

    public int GetSize()
    {
        return this.Size;
    }

    public int[] GetMatrix()
    {
        return this.Matrix;
    }

    public void Print(string path = "")
    {
        if (path != "")
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    file.Write(this.Matrix[i * this.Size + j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
            return;
        }
        for (int i = 0; i < this.Size; ++i)
        {
            for (int j = 0; j < this.Size; ++j)
                Console.Write(this.Matrix[i * this.Size + j] + " ");
            Console.WriteLine();
        }
    }
}

