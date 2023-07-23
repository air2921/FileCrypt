using System.Diagnostics;

namespace FileCrypt
{
    internal class DirectoryManager : IDirectoryManager
    {
        public void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-Command \"Remove-Item -Recurse -Force -LiteralPath '{directoryPath}'\"";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The {directoryPath} directory was deleted successfully.");
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
        public void CreateBackup(string sourceDirectory, string backupDirectory)
        {
            if(!Directory.Exists(backupDirectory) && Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(backupDirectory);

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
                Console.WriteLine("The backup was created successfully");
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
    }
}
