namespace FileCrypt
{
    internal class FileManager
    {
        internal string CheckFile(string filePath)
        {
            string fullPath = Path.GetFullPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);

            if (string.IsNullOrEmpty(fileExtension))
            {
                // Если расширение файла не указано, пробуем определить его
                string[] matchingFiles = Directory.GetFiles(Path.GetDirectoryName(fullPath), fileName + ".*");
                if (matchingFiles.Length > 0)
                {
                    filePath = matchingFiles[0];
                }
            }

            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                Console.WriteLine($"\nФайла по такому пути не существует     '{filePath}'");
                Environment.Exit(1); // Выход из программы с кодом ошибки
                return null;
            }
        }

        internal string CheckDirectory (string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                return directoryPath;
            }
            else
            {
                Console.WriteLine($"\nДиректории по такому пути не существует     '{directoryPath}'");
                Environment.Exit(1);
                return null;
            }
        }
    }
}