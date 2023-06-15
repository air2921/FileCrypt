using System.Security.Cryptography;

namespace FileCrypt
{
    internal class EncryptData : IEncryptor
    {
        private static void Encrypt(Stream source, Stream target, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] iv = aes.IV;
                target.Write(iv, 0, iv.Length);
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, iv, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using (CryptoStream cryptoStream = new CryptoStream(target, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    source.CopyTo(cryptoStream);
                }
            }
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