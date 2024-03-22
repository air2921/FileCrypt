namespace FileCrypt
{
    internal class OperationResultMessage : IOperationResultMessage
    {
        public void ResultMessage(int allFiles, int totalFiles)
        {
            ConsoleColor color;
            string message;

            if (allFiles == totalFiles && totalFiles > 0)
            {
                color = ConsoleColor.Green; message = "All files in the directory were processed successfully.";
            }
            else if (allFiles == 0 && totalFiles == 0)
            {
                color = ConsoleColor.Red; message = "No files were found in the directory to perform the operation, or an error occurred during the operation.";
            }
            else
            {
                color = ConsoleColor.Red; message = "Not all files were processed successfully.";
            }

            Console.ForegroundColor = color;
            Console.WriteLine("\n" + message);
        }
    }

    internal interface IOperationResultMessage
    {
        public void ResultMessage(int allFiles, int totalFiles);
    }
}