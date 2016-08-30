using System;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name:");
            string userName = Console.ReadLine();
            Console.WriteLine("Welcome, " + userName);
            Console.WriteLine();
            Console.WriteLine("There are 3 commands:");
            Console.WriteLine("\t\t 'send {your message}'");
            Console.WriteLine("\t\t 'help'");
            Console.WriteLine("\t\t 'exit'");

            const int PORT = 15000;
            var receiver = new Receiver(PORT);
            var helper = new Helper();
            var sender = new Sender(receiver, PORT);

            while (true)
            {
                string message = Console.ReadLine();
                string command = message.Split(' ')[0];
                message = message.Substring(message.IndexOf(' ') + 1);

                switch (command)
                {
                    case "send":
                        sender.Send('@' + userName + ": " + message, false);
                        break;

                    case "help":
                        helper.Help();
                        break;
                        
                    case "exit":
                        receiver.StopListening();
                        return;

                    default:
                        Console.WriteLine(command + " unknown command.");
                        break;
                }
            }
        }
    }
}
