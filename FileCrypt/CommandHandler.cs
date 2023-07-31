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
                return _key ??= getValue.GetKeyValueFromConfigurationFile();
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
                    Console.WriteLine("\nThis file path is not available");
                    Console.ReadKey();
                    Environment.Exit(5);
                }

                _pathTo = value;
            }
        }


        public void Help()
        {
            var HelpedCommands =
            "\nGENERATE           The command is used to create an encryption key\n\n" +
            "FENC               The command is used to start the process of encrypting a single file\n\n" +
            "FDEC               The command is used to start the process of decrypting a single file\n\n" +
            "DIRENC             The command is used to start the process of encrypting all files in the specified directory\n\n" +
            "DIRDEC             The command is used to start the process of decrypting all files in the specified directory\n\n" +
            "DIRBACKUP          The command is used to create a backup copy of the selected directory\n\n" +
            "DIRDEL             The command is used to force the removal of a directory\n\n" +
            "EX                 Command to get an example of a valid path input\n\n" +
            "EXIT               The command is used to exit the application" +
            "\n\n\n\nAll directory backups are stored in the path 'C:\\directory backups\\Your folder(Reserve)'";

            Console.WriteLine(HelpedCommands);
        }

        public void EncryptFile()
        {
            Console.WriteLine("Specify the path to the file you want to encrypt :");

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
                Console.WriteLine($"The file does not exist at this path: '{PathTo}'");
                return;
            }
        }

        public void DecryptFile()
        {
            Console.WriteLine("Specify the path to the file you want to decrypt :");

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
                Console.WriteLine($"The file does not exist at this path: '{PathTo}'");
                return;
            }
        }

        public void EncryptDirectory()
        {
            Console.WriteLine("Specify the path to the directory in which you want to encrypt all files :");

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
                        Console.WriteLine($"File encryption error {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, totalFiles);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThere is no directory for this path     '{PathTo}'");
                return;
            }
        }

        public void DecryptDirectory()
        {
            Console.WriteLine("Specify the path to the directory in which you want to decrypt all files :");

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
                        Console.WriteLine($"File decrypting error {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, totalFiles);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThere is no directory for this path     '{PathTo}'");
                return;
            }
        }

        public void Generate()
        {
            Console.WriteLine("\nIf you have already issued the GENERATE command, re-executing the command may lead to undesirable consequences" +
            "\nEnter 'OK' if you have not yet created an encryption key." +
            "\nEnter 'STOP' if you have already created an encryption key.");

            var Check = Console.ReadLine();
            if (Check == "OK")
            {
                saveValues.SaveValuesToConfigurationFile();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nThe value is set and can be used.");
            }
            else if (Check == "STOP")
            {
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis command is invalid");
                return;
            }
        }

        public void Example()
        {
            Console.WriteLine(
                "\nExample path to images directory             C:/Users/Username/Pictures/Name of your pictures folder\n\n" +
                "Example path to documents directory          C:/Users/Username/Documents/Documents folder name\n\n" +
                "Example path to video directory              C:/Users/Username/Videos/Name of your video folder\n\n" +
                "Example path to directory on a disk          C:/Your folder name/Folder name in folder (If available)\n\n" +
                "Example file path                            C:/Your folder name/File name (File extension is not required)");
        }

        public void CreateBackupDirectory()
        {
            Console.WriteLine("Specify the path to the directory for which you want to create a backup:");
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
                Console.WriteLine($"\nError while creating directory backup\n{ex.Message}");
                return;
            }
        }

        public void DeleteDirectory()
        {
            Console.WriteLine("Enter the path to the directory that you want to forcibly remove");
            var directoryPath = Console.ReadLine();
            try
            {
                PathTo = directoryPath;

                directoryManager.DeleteDirectory(PathTo);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThere is no directory for this path.     '{PathTo}'");
                return;
            }
        }
    }
}