﻿namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"FileCrypt ({Environment.UserName})";
            ICommands command = new CommandHandler();

            Console.WriteLine("Введите команду которую хотите выполнить");
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
                    case "DIRDEL":
                        command.DeleteDirectory();

                        break;
                    case "DIRBACKUP":
                        command.CreateBackupDirectory();

                        break;
                    case "EX":
                        command.Example();

                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nТакой команды не существует.\nВы можете ввести '.help' для получения справочной информации.");
                        Console.ResetColor();
                        break;

                }

                Console.ResetColor();
                Console.WriteLine("\n\nВведите следующую команду:");
                InputUser = Console.ReadLine();
            }    
        }
    }
}