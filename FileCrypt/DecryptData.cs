using System.Security.Cryptography;

namespace FileCrypt
{
    internal class DecryptData : IDecryptor
    {
        private static void Decrypt(Stream source, Stream target, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.IV.Length];
                source.Read(iv, 0, iv.Length);
                aes.IV = iv;
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, iv, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using (CryptoStream cryptoStream = new CryptoStream(source, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(target);
                }
            }
        }

        public void DecryptFile(string filePath, byte[] key)
        {
            string tmp = $"{filePath}.tmp";
            using (var source = File.OpenRead(filePath))
            using (var target = File.Create(tmp))
            {
                Decrypt(source, target, key);
            }
            File.Move(tmp, filePath, true);

            Console.WriteLine($"Файл {filePath} был успешно расшифрован.");
        }
    }

    public interface IDecryptor
    {
        void DecryptFile(string filePath, byte[] key);
    }
}