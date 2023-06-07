namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var HelpedCommands = "\nGENERATE           Команда используется для создания Ключа и Соли шифрования\n\n" +
                "FILE ENC           Команда используется для начала процесса шифрования отдельного файла\n\n" +
                "FILE DEC           Команда используется для начала процесса расшифровывания отдельного файла\n\n" +
                "DIRECTORY ENC      Команда используется для начала процесса шифрования всех файлов в указанной директории\n\n" +
                "DIRECTORY DEC      Команда используется для начала процесса расшифровывания всех файлов в указанной директории";

            ConfigurationFile configurationFile = new ConfigurationFile();
            ISaveValuesToConfigurationFile saveValues = new ConfigurationFile();
            IEncryptorTxtFile encryptTxt = new EncryptData();
            IDecryptorTxtFile decryptTxt = new DecryptData();
            IEncryptorImageFile encryptImage = new EncryptData();
            IDecryptorImageFile decryptImage = new DecryptData();
            FileManager fileManager = new FileManager();
            

            Console.WriteLine("Введите команду которую хотите выполнить");
            string InputUser = Console.ReadLine();
            switch (InputUser)
            {
                case ".help":
                    Console.WriteLine(HelpedCommands);
                    Console.ReadKey();
                    break;
                case "GENERATE":
                    Console.WriteLine("Если вы уже выполняли команду GENERATE, повторное выполнение команды может привести к нежелательным последствиям" +
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
                    break;
                case "FILE ENC":
                    Console.WriteLine("Укажите путь к файлу который вы хотите зашифровать :");
                    string InputEncryptFilePath = Console.ReadLine();

                    //Пытаемся получить расширение файла
                    var EncryptFileName = fileManager.CheckFile(InputEncryptFilePath);

                    byte[] EncryptKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] EncryptSalt = configurationFile.GetSaltValueFromConfigurationFile();
                    if (EncryptFileName.Contains(".txt"))
                    {
                        encryptTxt.EncryptTxtFile(EncryptFileName, EncryptKey, EncryptSalt);
                    }
                    else if (EncryptFileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || EncryptFileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        encryptImage.EncryptImageFile(EncryptFileName, EncryptKey, EncryptSalt);
                    }
                    break;
                case "FILE DEC":
                    Console.WriteLine("Укажите путь к файлу который вы хотите расшифровать :");
                    string InputDecryptedFilePath = Console.ReadLine();

                    //Пытаемся получить расширение файла
                    var DecryptedFileName = fileManager.CheckFile(InputDecryptedFilePath);

                    byte[] DecryptKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] DecryptSalt = configurationFile.GetSaltValueFromConfigurationFile();
                    if (DecryptedFileName.Contains(".txt"))
                    {
                        decryptTxt.DecryptTxtFile(DecryptedFileName, DecryptKey, DecryptSalt);
                    }
                    else if(DecryptedFileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || DecryptedFileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        decryptImage.DecryptImageFile(DecryptedFileName, DecryptKey, DecryptSalt);
                    }
                    break;
                case "DIRECTORY ENC":
                    Console.WriteLine("Укажите путь к директории в которой нужно зашифровать все файлы :");
                    string InputEncryptDirectoryPath = Console.ReadLine();

                    var EncryptDirectoryPath = fileManager.CheckDirectory(InputEncryptDirectoryPath);
                    string[] EncryptFileNames = Directory.GetFiles(EncryptDirectoryPath);

                    byte[] EncryptDirectoryKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] EncryptDirectorySalt = configurationFile.GetSaltValueFromConfigurationFile();
                    foreach (string fileName in EncryptFileNames)
                    {
                        if (fileName.Contains(".txt"))
                        {
                            encryptTxt.EncryptTxtFile(fileName, EncryptDirectoryKey, EncryptDirectorySalt);
                        }
                        else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            encryptImage.EncryptImageFile(fileName, EncryptDirectoryKey, EncryptDirectorySalt);
                        }
                    }
                    break;
                case "DIRECTORY DEC":
                    Console.WriteLine("Укажите путь к директории в которой нужно расшифровать все файлы :");
                    string InputDecryptDirectoryPath = Console.ReadLine();

                    var DecryptDirectoryPath = fileManager.CheckDirectory(InputDecryptDirectoryPath);
                    string[] DecryptFileNames = Directory.GetFiles(DecryptDirectoryPath);

                    byte[] DecryptDirectoryKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] DecryptDirectorySalt = configurationFile.GetSaltValueFromConfigurationFile();
                    foreach (string fileName in DecryptFileNames)
                    {
                        if (fileName.Contains(".txt"))
                        {
                            decryptTxt.DecryptTxtFile(fileName, DecryptDirectoryKey, DecryptDirectorySalt);
                        }
                        else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            decryptImage.DecryptImageFile(fileName, DecryptDirectoryKey, DecryptDirectorySalt);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Такой команды не существует, вы можете ввести '.help' для получения информации о применяемых командах");
                    Console.ReadKey();
                    break;

            }
        }
    }
}