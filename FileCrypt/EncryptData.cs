using System.Security.Cryptography;

namespace FileCrypt
{
    internal class EncryptData : IEncryptor
    {

        private static byte[] Encrypt(byte[] fileData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    encryptedStream.Write(salt, 0, salt.Length);
                    encryptedStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileData, 0, fileData.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    byte[] encryptedData = encryptedStream.ToArray();
                    return encryptedData;
                }
            }
        }

        public void EncryptFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] imageData = File.ReadAllBytes(filePath);
            byte[] encryptedData = Encrypt(imageData, key, salt);

            using (FileStream encryptedFileStream = new FileStream(filePath, FileMode.Create))
            {
                encryptedFileStream.Write(encryptedData, 0, encryptedData.Length);
            }
            Console.WriteLine($"Файл {filePath} был успешно зашифрован.");
        }
    }

    public interface IEncryptor
    {
        void EncryptFile(string filePath, byte[] key, byte[] salt);
    }
}