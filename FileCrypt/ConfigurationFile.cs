using System.Configuration;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileCrypt
{
    internal class ConfigurationFile : ISaveValuesToConfigurationFile, IGetValueFromConfigurationFile
    {
        public void SaveValuesToConfigurationFile()
        {
            GenerateRandomKey key = new GenerateRandomKey();
            var Key = key.GenerateKey();

            var encodedKey = Convert.ToBase64String(Key);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Add("Key", encodedKey);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public byte[] GetKeyValueFromConfigurationFile()
        {
            try
            {
                SetAdminOnlyAccess(GetConfigurationFilePath());

                var ValueKey = ConfigurationManager.AppSettings["Key"];

                if (String.IsNullOrWhiteSpace(ValueKey))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nКлюч не был найден." +
                        "\nЕсли вы уверены что вы уже генерировали ключ и соль, проверьте файл конфигурации." +
                        "\nЕсли в файле конфигурации отсутствует ключ, вставьте ранее сгенерированный ключ в поле Key");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                byte[] KeyBytes = Convert.FromBase64String(ValueKey);

                return KeyBytes;
            }
            catch (PrivilegeNotHeldException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nУ вас нет достаточных привилегий для выполнения этой операции.");
                Console.ReadKey();
                Environment.Exit(1);
                return null;
            }
        }

        private static void SetAdminOnlyAccess(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

#pragma warning disable CA1416 // Проверка совместимости платформы
            try
            {
                FileSecurity fileSecurity = new FileSecurity(filePath, AccessControlSections.All);

                SecurityIdentifier adminSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);

                FileSystemAccessRule adminAccessRule = new FileSystemAccessRule(adminSid, FileSystemRights.FullControl, AccessControlType.Allow);

                fileSecurity.AddAccessRule(adminAccessRule);

                fileInfo.SetAccessControl(fileSecurity);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nФайл конфигурации не был найден.\nРекомендуется произвести команду GENERATE, для создания файла конфигурации");
                Console.ReadKey();
                Environment.Exit(1);
            }
#pragma warning disable CA1416 // Проверка совместимости платформы
        }

        private static string GetConfigurationFilePath()
        {
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var DirectoryPath = Path.GetDirectoryName(appPath);
            return DirectoryPath + "\\FileCrypt.dll.config";
        }
    }

    public interface ISaveValuesToConfigurationFile
    {
        void SaveValuesToConfigurationFile();
    }

    public interface IGetValueFromConfigurationFile
    {
        byte[] GetKeyValueFromConfigurationFile();
    }
}