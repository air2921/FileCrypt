using System.Security.Cryptography;

namespace FileCrypt
{
    internal class DecryptData : IDecryptor
    {
        private static void Decrypt(Stream source, Stream target, byte[] key)
        {
            using var aes = Aes.Create();
            byte[] iv = new byte[aes.BlockSize / 8];
            source.Read(iv);
            aes.IV = iv;
            using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
            {
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
            }

            using CryptoStream cryptoStream = new(source, aes.CreateDecryptor(), CryptoStreamMode.Read);
            cryptoStream.CopyTo(target);
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

            Console.WriteLine($"The file {filePath} was successfully decrypted.");
        }
    }

    public interface IDecryptor
    {
        void DecryptFile(string filePath, byte[] key);
    }
}