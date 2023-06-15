using System.Runtime;
using System.Security.Cryptography;

namespace FileCrypt
{
    internal class DecryptData : IDecryptor
    {

        private static byte[] Decrypt(string filePath, byte[] key, byte[] salt)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);

            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encryptedData, salt.Length, iv, 0, iv.Length);
                aes.IV = iv;

                using (MemoryStream encryptedMemoryStream = new MemoryStream(encryptedData, salt.Length + iv.Length, encryptedData.Length - salt.Length - iv.Length))
                using (MemoryStream decryptedMemoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedMemoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptedMemoryStream);
                    }

                    byte[] decryptedData = decryptedMemoryStream.ToArray();
                    return decryptedData;
                }
            }
        }

        public void DecryptFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] decryptedData = Decrypt(filePath, key, salt);
            File.WriteAllBytes(filePath, decryptedData);
            Console.WriteLine($"Файл {filePath} был успешно расшифрован.");

            decryptedData = null;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }
    }

    public interface IDecryptor
    {
        void DecryptFile(string filePath, byte[] key, byte[] salt);
    }
}