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
            Console.WriteLine("\t\t 'exit'");
            Console.WriteLine("\t\t 'help'");

            const int PORT = 15000;
            var receiver = new Receiver(PORT);
            var helper = new Helper();
            var sender = new Sender(receiver, PORT);

            while (true)
            {
                string message = Console.ReadLine() + ' ';
                string command = message.Remove(4);
                message = message.Substring(5);

                switch (command)
                {
                    case "send":
                        sender.Send('@' + userName + ": " + message, false);
                        break;

                    case "exit":
                        receiver.StopListening();
                        return;

                    case "help":
                        helper.HelpPage();
                        break;

                    default:
                        Console.WriteLine(command + " unknown command.");
                        break;
                }
            }
        }
    }
}
