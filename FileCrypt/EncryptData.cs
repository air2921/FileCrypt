using System.Security.Cryptography;

namespace FileCrypt
{
    internal class EncryptData : IEncryptor
    {
        private static void Encrypt(Stream source, Stream target, byte[] key)
        {
            using var aes = Aes.Create();
            byte[] iv = aes.IV;
            target.Write(iv);
            using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
            {
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
            }

            using CryptoStream cryptoStream = new(target, aes.CreateEncryptor(), CryptoStreamMode.Write);
            source.CopyTo(cryptoStream);
        }

        public void EncryptFile(string filePath, byte[] key)
        {
            string tmp = $"{filePath}.tmp";
            using (var source = File.OpenRead(filePath))
            using (var target = File.Create(tmp))
            {
                Encrypt(source, target, key);
            }
            File.Move(tmp, filePath, true);

            Console.WriteLine($"Файл {filePath} был успешно зашифрован.");
        }
    }

    public interface IEncryptor
    {
        void EncryptFile(string filePath, byte[] key);
    }
}