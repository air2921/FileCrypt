namespace FileCrypt
{
    internal class FileManager
    {
        internal void CheckFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файла по такому пути не существует\n{filePath}");
            }
            else
            {
                return;
            }
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
