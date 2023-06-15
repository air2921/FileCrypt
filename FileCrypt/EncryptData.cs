using System.Runtime;
using System.Security.Cryptography;

namespace FileCrypt
{
    internal class EncryptData : IEncryptor
    {

        private static void Encrypt(string filePath, byte[] fileData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (FileStream encryptedFileStream = new FileStream(filePath, FileMode.Create))
                {
                    encryptedFileStream.Write(salt, 0, salt.Length);
                    encryptedFileStream.Write(iv, 0, iv.Length);

                    using (MemoryStream encryptedMemoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(encryptedMemoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(fileData, 0, fileData.Length);
                            cryptoStream.FlushFinalBlock();
                            encryptedMemoryStream.Seek(0, SeekOrigin.Begin);
                            encryptedMemoryStream.CopyTo(encryptedFileStream);
                        }
                    }
                }
            }
        }

        public void EncryptFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] data = File.ReadAllBytes(filePath);
            Encrypt(filePath, data, key, salt);
            Console.WriteLine($"Файл {filePath} был успешно зашифрован.");

            data = null;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }
    }

    public interface IEncryptor
    {
        void EncryptFile(string filePath, byte[] key, byte[] salt);
    }
}