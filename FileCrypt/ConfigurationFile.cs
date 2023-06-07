using System.Configuration;

namespace FileCrypt
{
    internal class ConfigurationFile : ISaveValuesToConfigurationFile
    {
        public void SaveValuesToConfigurationFile()
        {
            GenerateRandomKeySalt keySalt = new GenerateRandomKeySalt();
            var Key = keySalt.GenerateRandomKey();
            var Salt = keySalt.GenerateRandomSalt();

            var encodedKey = Convert.ToBase64String(Key);
            var encodedSalt = Convert.ToBase64String(Salt);

            // Создаем новый файл конфигурации
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Добавляем ключ и значение в файл конфигурации
            config.AppSettings.Settings.Add("Key", encodedKey);
            config.AppSettings.Settings.Add("Salt", encodedSalt);

            // Сохранение файла конфигурации
            config.Save(ConfigurationSaveMode.Modified);

            // Перезагрузка конфигурации
            ConfigurationManager.RefreshSection("appSettings");
        }

        public byte[] GetKeyValueFromConfigurationFile()
        {
            try
            {
                var ValueKey = ConfigurationManager.AppSettings["Key"];
                byte[] KeyBytes = Convert.FromBase64String(ValueKey);

                return KeyBytes;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Не удалось найти ключ шифрования, рекомендуется выполнить команду GENERATE перед шифрования");
                return null;
            }
        }

        public byte[] GetSaltValueFromConfigurationFile()
        {
            try
            {
                var ValueSalt = ConfigurationManager.AppSettings["Salt"];
                byte[] SaltBytes = Convert.FromBase64String(ValueSalt);

                return SaltBytes;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Не удалось найти соль шифрования, рекомендуется выполнить команду GENERATE перед шифрования");
                return null;
            }
        }
    }

    public interface ISaveValuesToConfigurationFile
    {
        void SaveValuesToConfigurationFile();
    }
}