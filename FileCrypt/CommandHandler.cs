namespace FileCrypt
{
    internal class CommandHandler : ICommands
    {
        readonly IEncryptor encrypt = new EncryptData();
        readonly IDecryptor decrypt = new DecryptData();
        readonly ISaveValuesToConfigurationFile saveValues = new ConfigurationFile();
        readonly IGetValueFromConfigurationFile getValue = new ConfigurationFile();
        readonly IDirectoryManager directoryManager = new DirectoryManager();
        readonly FileManager fileManager = new FileManager();
        readonly OperationResultMessage message = new OperationResultMessage();

        private byte[]? _key;
        private string? _pathTo;

        private byte[] Key
        {
            get
            {
                if(_key == null)
                {
                    return _key = getValue.GetKeyValueFromConfigurationFile();
                }
                return _key;
            }
        }

        private string PathTo
        {
            get
            {
                return _pathTo;
            }
            set
            {
                if (value.Contains("FileCrypt"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nЭтот путь недоступен");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                _pathTo = value;
            }
        }


        public void Help()
        {
            var HelpedCommands =
            "\nGENERATE           Команда используется для создания ключа шифрования\n\n" +
            "FENC               Команда используется для начала процесса шифрования отдельного файла\n\n" +
            "FDEC               Команда используется для начала процесса расшифровывания отдельного файла\n\n" +
            "DIRENC             Команда используется для начала процесса шифрования всех файлов в указанной директории\n\n" +
            "DIRDEC             Команда используется для начала процесса расшифровывания всех файлов в указанной директории\n\n" +
            "DIRBACKUP          Команда используется для создания резервной копии выбранной директории\n\n" +
            "DIRDEL             Команда используется для принудительного удаления директории\n\n" +
            "EX                 Команда для получения примера правильного ввода пути\n\n" +
            "EXIT               Команда используется для выхода из приложения" +
            "\n\n\n\nВсе резервные копии директорий сохраняются по пути 'C:\\directory backups\\Ваша папка(Reserve)'";

            Console.WriteLine(HelpedCommands);
        }

        public void EncryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите зашифровать :");

            var FilePath = Console.ReadLine();
            try
            {
                PathTo = FilePath;
                byte[] key = Key;
                var FullNameFile = fileManager.GetFileExtension(PathTo);

                Console.ForegroundColor = ConsoleColor.Green;
                encrypt.EncryptFile(FullNameFile, key);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Файла по такому пути не существует: '{PathTo}'");
                return;
            }
        }

        public void DecryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите расшифровать :");

            var FilePath = Console.ReadLine();
            try
            {
                PathTo = FilePath;
                byte[] key = Key;
                var FullNameFile = fileManager.GetFileExtension(PathTo);

                Console.ForegroundColor = ConsoleColor.Green;
                decrypt.DecryptFile(FullNameFile, key);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Файла по такому пути не существует: '{PathTo}'");
                return;
            }
        }

        public void EncryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно зашифровать все файлы :");

            var DirectoryPath = Console.ReadLine();
            try
            {
                PathTo = DirectoryPath;

                string[] fileNames = Directory.GetFiles(PathTo, "*", SearchOption.AllDirectories);
                var totalFiles = fileNames.Length;
                var allFiles = 0;

                byte[] key = Key;

                foreach (string fileName in fileNames)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        encrypt.EncryptFile(fileName, key);
                        allFiles++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка при шифровании файла {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, totalFiles);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nДиректории по такому пути не существует     '{PathTo}'");
                return;
            }
        }

        public void DecryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно расшифровать все файлы :");

            var DirectoryPath = Console.ReadLine();
            try
            {
                PathTo = DirectoryPath;

                string[] fileNames = Directory.GetFiles(PathTo, "*", SearchOption.AllDirectories);
                var totalFiles = fileNames.Length;
                var allFiles = 0;

                byte[] key = Key;

                foreach (string fileName in fileNames)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        decrypt.DecryptFile(fileName, key);
                        allFiles++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка при расшифровке файла {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, totalFiles);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nДиректории по такому пути не существует     '{PathTo}'");
                return;
            }
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nЗначение установлено и может быть использовано.");
            }
            else if (Check == "STOP")
            {
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nТакой команды не предоставляется");
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
        }

        public void CreateBackupDirectory()
        {
            Console.WriteLine("Укажите путь к директории для которой нужно создать резервную копию:");
            var sourceDirectory = Console.ReadLine();
            try
            {
                var directoryName = new DirectoryInfo(sourceDirectory).Name;
                var directoryBackupName = $"C:/directory backups/{directoryName}(Reserve)";
                directoryManager.CreateBackup(sourceDirectory, directoryBackupName);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка при создании резервной копии директории\n{ex.Message}");
                return;
            }
        }

        public void DeleteDirectory()
        {
            Console.WriteLine("Введите путь к директории которую требуется насильно удалить");
            var directoryPath = Console.ReadLine();
            try
            {
                PathTo = directoryPath;

                directoryManager.DeleteDirectory(PathTo);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nДиректории по такому пути не существует     '{PathTo}'");
                return;
            }
        }
    }
}