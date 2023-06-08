namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandHandler command = new CommandHandler();

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
                case "FILE ENC":
                    command.EncryptFile();

                    break;
                case "FILE DEC":
                    command.DecryptFile();

                    break;
                case "DIRECTORY ENC":
                    command.EncryptDirectory();

                    break;
                case "DIRECTORY DEC":
                    command.DecryptDirectory();

                    break;
                default:
                    Console.WriteLine("Такой команды не существует, вы можете ввести '.help' для получения информации о применяемых командах");
                    Console.ReadKey();
                    break;

            }
        }
    }
}