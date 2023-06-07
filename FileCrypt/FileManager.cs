namespace FileCrypt
{
    internal class FileManager
    {
        internal string CheckFile(string filePath)
        {
            try
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
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файла по такому пути не существует\n{filePath}");
            }

            return filePath;
        }

        internal void CheckDirectory (string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Директории по такому пути не существует\n{directoryPath}");
            }
            else
            {
                return;
            }
        }
    }
}