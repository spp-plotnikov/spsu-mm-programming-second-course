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

            Console.Write("Enter Strategy (1 - all rates for lose player,\n 2 - all rates for win player, 3 - all rates for drow): ");
            while (1 == 1)
            {
                int str = Convert.ToInt32(Console.ReadLine());
                GameLogic gm = new GameLogic(str);
                gm.Game();
                Console.WriteLine("Result game: {0} ye", gm.resultGame);
                Console.WriteLine();
                Console.Write("For continue enter Strategy:");
            }
        }
    }
    
}
