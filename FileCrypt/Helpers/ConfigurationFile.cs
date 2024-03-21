using System.Configuration;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileCrypt.Helpers
{
    internal class ConfigurationFile : IConfiguration
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

                if (string.IsNullOrWhiteSpace(ValueKey))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nThe key was not found." +
                        "\nIf you are sure that you have already generated the key, check the configuration file." +
                        "\nIf there is no key in the configuration file, paste the previously generated key in the 'Key' field");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                byte[] KeyBytes = Convert.FromBase64String(ValueKey);

                return KeyBytes;
            }
            catch (PrivilegeNotHeldException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou do not have sufficient privileges to perform this operation.");
                Console.ReadKey();
                Environment.Exit(5);
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
                Console.WriteLine("\nConfiguration file was not found.\nIt is recommended to issue the GENERATE command to create a configuration file");
                Console.ReadKey();
                Environment.Exit(2);
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

    public interface IConfiguration
    {
        void SaveValuesToConfigurationFile();
        byte[] GetKeyValueFromConfigurationFile();
    }
}