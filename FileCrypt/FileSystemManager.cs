namespace FileCrypt
{
    internal class FileSystemManager
    {
        internal string GetFileExtension(string filePath)
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
            return filePath;
        }

        public string CheckFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
