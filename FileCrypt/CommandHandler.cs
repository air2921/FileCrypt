using FileCrypt.Cryptography;
using FileCrypt.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace FileCrypt
{
    internal class CommandHandler : ICommands
    {
        private readonly ICypher _encrypt;
        private readonly ICypher _decrypt;
        private readonly IConfiguration _configuration;

        public CommandHandler(
            [FromKeyedServices("encrypt")] ICypher encrypt,
            [FromKeyedServices("decrypt")] ICypher decrypt,
            IConfiguration configuration)
        {
            _encrypt = encrypt;
            _decrypt = decrypt;
            _configuration = configuration;
        }

        private string _path;
        private byte[] _key;


        private byte[] Key
        {
            get
            {
                return _key ??= _configuration.GetKeyValueFromConfigurationFile();
            }
        }

        private string Path
        {
            get
            {
                return _path;
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

                _path = value;
            }
        }

        public void EncryptFile()
        {
            Console.WriteLine("Specify the path to the file you want to encrypt :");
            try
            {
                var Path = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Green;
                _encrypt.CypherFileAsync(Path, Key);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The file does not exist at this path: '{Path}'");
                return;
            }
        }

        public void DecryptFile()
        {
            Console.WriteLine("Specify the path to the file you want to decrypt :");

            Path = Console.ReadLine();
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                _decrypt.CypherFileAsync(Path, Key);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The file does not exist at this path: '{Path}'");
                return;
            }
        }

        public void EncryptDirectory()
        {
            Console.WriteLine("Specify the path to the directory in which you want to encrypt all files :");

            Path = Console.ReadLine();
            try
            {
                string[] files = Directory.GetFiles(Path, "*", SearchOption.AllDirectories);
                var allFiles = 0;

                foreach (string fileName in files)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        _encrypt.CypherFileAsync(fileName, Key);
                        allFiles++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"File encryption error {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, files.Length);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThere is no directory for this path     '{Path}'");
                return;
            }
        }

        public void DecryptDirectory()
        {
            Console.WriteLine("Specify the path to the directory in which you want to decrypt all files :");

            Path = Console.ReadLine();
            try
            {
                string[] files = Directory.GetFiles(Path, "*", SearchOption.AllDirectories);
                var allFiles = 0;

                foreach (string fileName in files)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        _decrypt.CypherFileAsync(fileName, Key);
                        allFiles++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"File decrypting error {fileName}: {ex.Message}");
                    }
                }
                message.ResultMessage(allFiles, files.Length);
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThere is no directory for this path     '{Path}'");
                return;
            }
        }

        public void Generate()
        {
            Console.WriteLine("\nIf you have already issued the GENERATE command, re-executing the command may lead to undesirable consequences" +
            "\nEnter 'OK' if you have not yet created an encryption key." +
            "\nEnter 'STOP' if you have already created an encryption key.");

            var Check = Console.ReadLine();
            if (Check != "OK" && Check != "STOP")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis command is invalid");
                return;
            }
            else
            {
                if (Check == "STOP")
                    return;

                _configuration.SaveValuesToConfigurationFile();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nThe value is set and can be used.");
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

        public void Help()
        {
            var HelpedCommands =
            "\nGENERATE           The command is used to create an encryption key\n\n" +
            "FENC               The command is used to start the process of encrypting a single file\n\n" +
            "FDEC               The command is used to start the process of decrypting a single file\n\n" +
            "DIRENC             The command is used to start the process of encrypting all files in the specified directory\n\n" +
            "DIRDEC             The command is used to start the process of decrypting all files in the specified directory\n\n" +
            "EX                 Command to get an example of a valid path input\n\n" +
            "EXIT               The command is used to exit the application" +
            "\n\n\n\nAll directory backups are stored in the path 'C:\\directory backups\\Your folder(Reserve)'";

            Console.WriteLine(HelpedCommands);
        }
    }
}