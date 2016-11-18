using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
public class TaskFour
{
    static void Main(String[] args)
    {
        ThreadPool pool = new ThreadPool();
        pool.Start();
        Console.WriteLine("Please, press key to stop this program");
        Tasks tasks = new Tasks(pool);
        Console.ReadKey();
        Console.WriteLine("....");
        tasks.Close();
        pool.Dispose();
        Console.ReadLine();
    }
}