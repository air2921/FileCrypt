namespace FileCrypt
{
    internal class FileManager
    {
        internal string GetFileExtension(string filePath)
        {
            string fullPath = Path.GetFullPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);

            if (string.IsNullOrEmpty(fileExtension))
            {
                string[] matchingFiles = Directory.GetFiles(Path.GetDirectoryName(fullPath), fileName + ".*");
                if (matchingFiles.Length > 0)
                {
                    return matchingFiles[0];
                }
            }
            return filePath;
        }
    }
}
