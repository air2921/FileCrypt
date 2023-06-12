using System.Diagnostics;

namespace FileCrypt
{
    internal class DirectoryAndFileOperations : IDirectoryOperations
    {
        public void DeleteDirectory(string directoryPath)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c rd /s /q \"{directoryPath}\"";
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                Console.WriteLine($"Директория {directoryPath} успешно удалена.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Ошибка удаления директории {directoryPath}: отсутствуют необходимые разрешения доступа.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка при попытке удаления директории {ex.Message}");
            }
        }
        public void CreateBackup(string sourceDirectory, string backupDirectory)
        {
            CreateDirectory(backupDirectory);

            try
            {
                string[] files = Directory.GetFiles(sourceDirectory);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(backupDirectory, fileName);
                    File.Copy(file, destPath, true);
                }

                string[] directories = Directory.GetDirectories(sourceDirectory);
                foreach (string directory in directories)
                {
                    string directoryName = Path.GetFileName(directory);
                    string destPath = Path.Combine(backupDirectory, directoryName);
                    CreateBackup(directory, destPath);
                }
                Console.WriteLine("Резервная копия была создана успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании резервной копии директории: {ex.Message}");
            }
            //var backupDirectory = $"C:/{sourceDirectory}(Reserve)";
        }

        private static void CreateDirectory(string backupDirectory)
        {
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }
        }
    }
    public interface IDirectoryOperations
    {
        void CreateBackup(string sourceDirectory, string backupDirectory);
        void DeleteDirectory(string directoryPath);
    }
}
