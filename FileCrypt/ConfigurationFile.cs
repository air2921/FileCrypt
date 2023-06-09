using System.Configuration;

namespace FileCrypt
{
    internal class ConfigurationFile : ISaveValuesToConfigurationFile, IGetValueFromConfigurationFile
    {
        public void SaveValuesToConfigurationFile()
        {
            GenerateRandomKeySalt keySalt = new GenerateRandomKeySalt();
            var Key = keySalt.GenerateRandomKey();
            var Salt = keySalt.GenerateRandomSalt();

            var encodedKey = Convert.ToBase64String(Key);
            var encodedSalt = Convert.ToBase64String(Salt);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Add("Key", encodedKey);
            config.AppSettings.Settings.Add("Salt", encodedSalt);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public byte[] GetKeyValueFromConfigurationFile()
        {
            var ValueKey = ConfigurationManager.AppSettings["Key"];

            if (ValueKey == null)
            {
                Console.WriteLine("\nКлюч не был найден.\nРекомендуется произвести команду GENERATE, для создания ключа и соли\n" +
                    "Если вы уверены что вы уже генерировали ключ и соль, проверьте файл конфигурации.\n" +
                    "Если в файле конфигурации отсутствует ключ, вставьте ранее сгенерированный ключ в поле Key");
                Environment.Exit(1);
            }

            byte[] KeyBytes = Convert.FromBase64String(ValueKey);

            return KeyBytes;
        }

        public byte[] GetSaltValueFromConfigurationFile()
        {
            var ValueSalt = ConfigurationManager.AppSettings["Salt"];

            if(ValueSalt == null)
            {
                Console.WriteLine("\nСоль не была найдена.\nРекомендуется произвести команду GENERATE, для создания ключа и соли\n" +
                    "Если вы уверены что вы уже генерировали ключ и соль, проверьте файл конфигурации.\n" +
                    "Если в файле конфигурации отсутствует соль, вставьте ранее сгенерированную соль в поле Salt");
            }

            byte[] SaltBytes = Convert.FromBase64String(ValueSalt);

            return SaltBytes;
        }
    }

    public interface ISaveValuesToConfigurationFile
    {
        void SaveValuesToConfigurationFile();
    }

    public interface IGetValueFromConfigurationFile
    {
        byte[] GetKeyValueFromConfigurationFile();
        byte[] GetSaltValueFromConfigurationFile();
    }
}