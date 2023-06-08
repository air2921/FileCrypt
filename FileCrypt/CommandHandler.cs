namespace FileCrypt
{
    internal class CommandHandler
    {
        ConfigurationFile configurationFile = new ConfigurationFile();
        IEncryptor encrypt = new EncryptData();
        IDecryptor decrypt = new DecryptData();
        ISaveValuesToConfigurationFile saveValues = new ConfigurationFile();
        FileManager fileManager = new FileManager();

        public void Help()
        {
            var HelpedCommands = "\nGENERATE           Команда используется для создания Ключа и Соли шифрования\n\n" +
            "FENC               Команда используется для начала процесса шифрования отдельного файла\n\n" +
            "FDEC               Команда используется для начала процесса расшифровывания отдельного файла\n\n" +
            "DIRENC             Команда используется для начала процесса шифрования всех файлов в указанной директории\n\n" +
            "DIRDEC             Команда используется для начала процесса расшифровывания всех файлов в указанной директории\n\n" +
            "EX                 Команда для получения примера правильного ввода пути";

            Console.WriteLine(HelpedCommands);
            Console.ReadKey();
        }

        public void EncryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите зашифровать :");

            string FilePath = Console.ReadLine();

            //Пытаемся получить расширение файла
            var FileName = fileManager.CheckFile(FilePath);

            byte[] EncryptKey = configurationFile.GetKeyValueFromConfigurationFile();
            byte[] EncryptSalt = configurationFile.GetSaltValueFromConfigurationFile();

            encrypt.EncryptFile(FileName, EncryptKey, EncryptSalt);
        }

        public void DecryptFile()
        {
            Console.WriteLine("Укажите путь к файлу который вы хотите расшифровать :");

            string FilePath = Console.ReadLine();

            //Пытаемся получить расширение файла
            var FileName = fileManager.CheckFile(FilePath);

            byte[] DecryptKey = configurationFile.GetKeyValueFromConfigurationFile();
            byte[] DecryptSalt = configurationFile.GetSaltValueFromConfigurationFile();

            decrypt.DecryptFile(FileName, DecryptKey, DecryptSalt);
        }

        public void EncryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно зашифровать все файлы :");

            string DirectoryPath = Console.ReadLine();

            var DirectoryName = fileManager.CheckDirectory(DirectoryPath);
            string[] FileNames = Directory.GetFiles(DirectoryName);

            byte[] EncryptDirectoryKey = configurationFile.GetKeyValueFromConfigurationFile();
            byte[] EncryptDirectorySalt = configurationFile.GetSaltValueFromConfigurationFile();
            foreach (string fileName in FileNames)
            {
                encrypt.EncryptFile(fileName, EncryptDirectoryKey, EncryptDirectorySalt);
            }
        }

        public void DecryptDirectory()
        {
            Console.WriteLine("Укажите путь к директории в которой нужно расшифровать все файлы :");

            string DirectoryPath = Console.ReadLine();

            var DirectoryName = fileManager.CheckDirectory(DirectoryPath);
            string[] FileNames = Directory.GetFiles(DirectoryName);

            byte[] DecryptDirectoryKey = configurationFile.GetKeyValueFromConfigurationFile();
            byte[] DecryptDirectorySalt = configurationFile.GetSaltValueFromConfigurationFile();
            foreach (string fileName in FileNames)
            {
                decrypt.DecryptFile(fileName, DecryptDirectoryKey, DecryptDirectorySalt);
            }
        }

        public void Generate()
        {
            Console.WriteLine(" Если вы уже выполняли команду GENERATE, повторное выполнение команды может привести к нежелательным последствиям" +
            "\n Введите 'OK' если вы еще не создавали ключ шифрования." +
            "\n Введите 'STOP' если вы уже создавали ключ шифрования.\n");

            var InputCheck = Console.ReadLine();
            if (InputCheck == "OK")
            {
                saveValues.SaveValuesToConfigurationFile();
            }
            else if (InputCheck == "STOP")
            {
                return;
            }
        }

        public  void Example()
        {
            Console.WriteLine(
                "\nПример пути к папке с изображениями          C:/Users/Имя пользователя/Pictures/Название вашей папки с изображениями\n\n" +
                "Пример пути к папке с документами            C:/Users/Имя пользователя/Documents/Название папки с документами\n\n" +
                "Пример пути к папке с видео                  C:/Users/Имя пользователя/Videos/Название вашей папки с видео\n\n" +
                "Пример пути к папке находящейся диске        C:/Название вашей папки/Название папки в папке [(При наличии)]\n\n" +
                "Пример пути к файлу                          С/Название вашей папки/VashFile [(Расширение файла указывать не нужно)]\n");
            Console.ReadKey();
        }
    }
}
