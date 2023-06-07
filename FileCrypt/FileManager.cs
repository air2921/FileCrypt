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

        internal string CheckDirectory (string directoryPath)
        {
            try
            {
                if(Directory.Exists(directoryPath))
                {
                    return directoryPath;
                }
            }
            catch (DirectoryNotFoundException)
            {
                return ($"Директории по такому пути не существует\n{directoryPath}");
            }
            return directoryPath;
        }
    }
}