namespace FileCrypt
{
    internal class OperationResultMessage
    {
        public void ResultMessage(int allFiles, int totalFiles)
        {
            if (allFiles == totalFiles && totalFiles > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAll files in the directory were processed successfully.");
            }
            else if (allFiles == 0 && totalFiles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo files were found in the directory to perform the operation, or an error occurred during the operation.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNot all files were processed successfully.");
            }
        }
    }
}