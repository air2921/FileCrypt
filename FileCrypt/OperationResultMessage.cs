namespace FileCrypt
{
    internal class OperationResultMessage
    {
        public void ResultMessage(int allFiles, int totalFiles)
        {
            if (allFiles == totalFiles && totalFiles > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nВсе файлы в директории были успешно обработаны.");
            }
            else if (allFiles == 0 && totalFiles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nВ директории не найдено файлов для выполнения операции или при выполнении операции произошла ошибка.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nНе все файлы удалось успешно обработать.");
            }
        }
    }
}