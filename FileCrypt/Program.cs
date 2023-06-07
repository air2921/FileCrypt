namespace FileCrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var HelpedCommands = "GENERATE     Команда используется для создания Ключа и Соли шифрования\n" +
                "ENCRYPT      Команда используется для начала процесса шифрования с использованием ключа и соли\n" +
                "DECRYPT      Команда используется для начала процесса расшифрования с использованием ключа и соли";

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
                case "ENCRYPT":
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
                case "DECRYPT":
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
                default:
                    Console.WriteLine("Такой команды не существует, вы можете ввести '.help' для получения информации о применяемых командах");
                    Console.ReadKey();
                    break;

            }
        }
    }
}