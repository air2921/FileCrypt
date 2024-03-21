namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"FileCrypt ({Environment.UserName})";
            ICommands command = new CommandHandler();

            Console.WriteLine("Enter the command you want to run");
            string InputUser = Console.ReadLine();
            while (InputUser != "EXIT")
            {
                switch (InputUser)
                {
                    case ".help":
                        command.Help();

                        break;
                    case "GENERATE":
                        command.Generate();

                        break;
                    case "FENC":
                        command.EncryptFile();

                        break;
                    case "FDEC":
                        command.DecryptFile();

                        break;
                    case "DIRENC":
                        command.EncryptDirectory();

                        break;
                    case "DIRDEC":
                        command.DecryptDirectory();

                        break;
                    case "EX":
                        command.Example();

                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nSuch command does not exist, type '.help' for information on valid commands.");
                        Console.ResetColor();
                        break;

                }

                Console.ResetColor();
                Console.WriteLine("\n\nEnter your next command:");
                InputUser = Console.ReadLine();
            }    
        }
    }
}