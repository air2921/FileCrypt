namespace FileCrypt
{
    internal interface ICommands
    {
        void Help();
        void EncryptFile();
        void DecryptFile();
        void Generate();
        void Example();
        void EncryptDirectory();
        void DecryptDirectory();
    }
}