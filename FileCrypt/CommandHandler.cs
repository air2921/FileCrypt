﻿namespace FileCrypt
{
    internal class CommandHandler : ICommands
    {
        readonly IEncryptor encrypt = new EncryptData();
        readonly IDecryptor decrypt = new DecryptData();
        readonly ISaveValuesToConfigurationFile saveValues = new ConfigurationFile();
        readonly IGetValueFromConfigurationFile getValue = new ConfigurationFile();
        readonly IDirectoryOperations directoryOperations = new DirectoryAndFileOperations();
        readonly FileManager fileManager = new FileManager();
        readonly OperationResultMessage message = new OperationResultMessage();

        private byte[]? _key;
        private byte[] Key
        {
            get
            {
                return _key = getValue.GetKeyValueFromConfigurationFile();
            }
        }

        private byte[]? _salt;
        private byte[] Salt
        {
            get
            {
                return _salt = getValue.GetSaltValueFromConfigurationFile();
            }
        }

        public void Help()
        {
            var HelpedCommands =
            "\nGENERATE           Команда используется для создания Ключа и Соли шифрования\n\n" +
            "FENC               Команда используется для начала процесса шифрования отдельного файла\n\n" +
            "FDEC               Команда используется для начала процесса расшифровывания отдельного файла\n\n" +
            "DIRENC             Команда используется для начала процесса шифрования всех файлов в указанной директории\n\n" +
            "DIRDEC             Команда используется для начала процесса расшифровывания всех файлов в указанной директории\n\n" +
            "DIRBACKUP          Команда используется для создания резервной копии выбранной директории\n\n" +
            "DIRDEL             Команда используется для принудительного удаления директории\n\n" +
            "EX                 Команда для получения примера правильного ввода пути" +
            "\n\n\nЕсли программа закрывается при попытке шифрования или расшифровки, попробуйте запустить утилиту от имени Администратора!";

            Console.WriteLine(HelpedCommands);
            Console.ReadKey();
        }

        public void EncryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите зашифровать :");

            string FilePath = Console.ReadLine();

            var FileName = fileManager.CheckFile(FilePath);

            byte[] key = Key;
            byte[] salt = Salt;

            encrypt.EncryptFile(FileName, key, salt);
            Console.ReadKey();
        }

        public void DecryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите расшифровать :");

            string FilePath = Console.ReadLine();

            var FileName = fileManager.CheckFile(FilePath);

            byte[] key = Key;
            byte[] salt = Salt;

            decrypt.DecryptFile(FileName, key, salt);
            Console.ReadKey();
        }

        public void EncryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно зашифровать все файлы :");

            string DirectoryPath = Console.ReadLine();

            var directoryName = fileManager.CheckDirectory(DirectoryPath);
            string[] fileNames = Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories);
            var totalFiles = fileNames.Length;
            var allFiles = 0;

            byte[] key = Key;
            byte[] salt = Salt;

            foreach (string fileName in fileNames)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    encrypt.EncryptFile(fileName, key, salt);
                    allFiles++;
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка при шифровании файла {fileName}: {ex.Message}");
                    Console.ResetColor();
                }
            }
            message.ResultMessage(allFiles, totalFiles);
        }

        public void DecryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно расшифровать все файлы :");

            string DirectoryPath = Console.ReadLine();

            var directoryName = fileManager.CheckDirectory(DirectoryPath);
            string[] fileNames = Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories);
            var totalFiles = fileNames.Length;
            var allFiles = 0;

            byte[] key = Key;
            byte[] salt = Salt;

            foreach (string fileName in fileNames)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    decrypt.DecryptFile(fileName, key, salt);
                    allFiles++;
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка при расшифровке файла {fileName}: {ex.Message}");
                    Console.ResetColor();
                }
            }
            message.ResultMessage(allFiles, totalFiles);
        }

        public void Generate()
        {
            Console.WriteLine(" Если вы уже выполняли команду GENERATE, повторное выполнение команды может привести к нежелательным последствиям" +
            "\n Введите 'OK' если вы еще не создавали ключ шифрования." +
            "\n Введите 'STOP' если вы уже создавали ключ шифрования.\n");

            var Check = Console.ReadLine();
            if (Check == "OK")
            {
                saveValues.SaveValuesToConfigurationFile();
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine("\nЗначения установлены и могут быть использованы.");
                Console.ReadKey();
            }
            else if (Check == "STOP")
            {
                return;
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("\nТакой команды не предоставляется");
                Console.ReadKey();
                return;
            }
        }

        public void Example()
        {
            Console.WriteLine(
                "\nПример пути к папке с изображениями          C:/Users/Имя пользователя/Pictures/Название вашей папки с изображениями\n\n" +
                "Пример пути к папке с документами            C:/Users/Имя пользователя/Documents/Название папки с документами\n\n" +
                "Пример пути к папке с видео                  C:/Users/Имя пользователя/Videos/Название вашей папки с видео\n\n" +
                "Пример пути к папке находящейся диске        C:/Название вашей папки/Название папки в папке [(При наличии)]\n\n" +
                "Пример пути к файлу                          С/Название вашей папки/Название файла [(Расширение файла указывать не нужно)]\n");
            Console.ReadKey();
        }

        public void CreateBackupDirectory()
        {
            Console.WriteLine("Укажите путь к директории для которой нужно создать резервную копию:");
            var sourceDirectory = Console.ReadLine();
            var directoryName = fileManager.CheckDirectory(sourceDirectory);
            var backupDirectoryName = $"{directoryName}(Reserve)";
            var backupDirectory = Path.Combine("C:/directories backup", backupDirectoryName);
            directoryOperations.CreateBackup(sourceDirectory, backupDirectory);
            Console.ReadKey();
        }

        public void DeleteDirectory()
        {
            Console.WriteLine("Введите путь к директории которую требуется насильно удалить");
            var directoryPath = Console.ReadLine();
            var directoryName = fileManager.CheckDirectory(directoryPath);
            directoryOperations.DeleteDirectory(directoryName);
            Console.ReadKey();
        }
    }
}