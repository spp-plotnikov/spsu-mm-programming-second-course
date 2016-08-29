using System;


namespace Baccarat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start money = 1000 ye");
            Console.WriteLine("Rate = 50 ye");
            Console.WriteLine("Time: 40 games");
            Console.WriteLine();

            Console.Write("Enter Strategy (1 - all rates for lose player,\n 2 - all rates for win player, 3 - all rates for drow), ");
            Console.Write("or enter 4 to exit: ");
            while (true)
            {
                try {
                    int str = Convert.ToInt32(Console.ReadLine());
                    if (str == 4)
                        break;
                    if (str > 3 || str < 1)
                    {
                        Console.WriteLine("Error, try again");
                        continue;
                    }
                    GameLogic gm = new GameLogic(str);
                    Console.WriteLine();
                    Console.WriteLine("Result game: {0} ye", gm.Play());
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.Write("For continue enter Strategy: ");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error, try again");
                }
            }
        }
    }
    
}
