namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICommands command = new CommandHandler();

            Console.WriteLine("Введите команду которую хотите выполнить");
            string InputUser = Console.ReadLine();
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
                    Console.WriteLine("Такой команды не существует, вы можете ввести '.help' для получения информации о применяемых командах");
                    Console.ReadKey();
                    break;

            }
        }
    }
}