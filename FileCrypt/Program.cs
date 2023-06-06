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
            EncryptData encrypt = new EncryptData();
            DecryptData decrypt = new DecryptData();
            FileManager fileManager = new FileManager();
            

            Console.WriteLine("Введите команду которую хотите выполнить");
            string InputUser = Console.ReadLine();
            switch (InputUser)
            {
                case ".help":
                    Console.WriteLine(HelpedCommands);
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
                    string EncryptFilePath = Console.ReadLine();
                    fileManager.CheckFile(EncryptFilePath);
                    byte[] EncryptKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] EncryptSalt = configurationFile.GetSaltValueFromConfigurationFile();
                    encrypt.EncryptFile(EncryptFilePath, EncryptKey, EncryptSalt);
                    break;
                case "DECRYPT":
                    Console.WriteLine("Укажите путь к файлу который вы хотите расшифровать :");
                    string DecryptedFilePath = Console.ReadLine();
                    fileManager.CheckFile(DecryptedFilePath);
                    byte[] DecryptKey = configurationFile.GetKeyValueFromConfigurationFile();
                    byte[] DecryptSalt = configurationFile.GetSaltValueFromConfigurationFile();
                    decrypt.DecryptFile(DecryptedFilePath, DecryptKey, DecryptSalt);
                    break;
                default:
                    Console.WriteLine("Такой команды не существует, вы можете ввести '.help' для получения информации о применяемых командах");
                    break;

            }
        }
    }
}