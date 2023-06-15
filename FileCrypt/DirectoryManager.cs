using System.Diagnostics;
using System.IO;

namespace FileCrypt
{
    internal class DirectoryManager : IDirectoryManager
    {
        public void DeleteDirectory(string directoryPath)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-Command \"Remove-Item -Recurse -Force -LiteralPath '{directoryPath}'\"";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Директория {directoryPath} успешно удалена.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Непредвиденная ошибка при попытке удаления директории {ex.Message}");
            }
        }
        public void CreateBackup(string sourceDirectory, string backupDirectory)
        {
            FileSystemManager manager = new FileSystemManager();

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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Резервная копия была создана успешно");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при создании резервной копии директории: '{sourceDirectory}'\n\n{ex.Message}");
            }
        }

        public void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public string CheckDirectory(string directoryPath)
        {
            if(Directory.Exists(directoryPath))
            {
                return directoryPath;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
    public interface IDirectoryManager
    {
        void CreateBackup(string sourceDirectory, string backupDirectory);
        void DeleteDirectory(string directoryPath);
        string CheckDirectory(string directoryPath);
    }
}
