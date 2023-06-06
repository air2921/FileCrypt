using System.Security.Cryptography;
using System.Text;

namespace FileCrypt
{
    internal class DecryptData
    {
        private static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);

                    byte[] iv = new byte[aes.IV.Length];
                    Buffer.BlockCopy(encryptedData, salt.Length, iv, 0, iv.Length);
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] encryptedText = new byte[encryptedData.Length - salt.Length - iv.Length];
                        Buffer.BlockCopy(encryptedData, salt.Length + iv.Length, encryptedText, 0, encryptedText.Length);

                        byte[] decryptedData = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
                        return decryptedData;
                    }
                }
            }
        }

        internal void DecryptTextFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = Decrypt(encryptedData, key, salt);
            string decryptedText = Encoding.UTF8.GetString(decryptedData);
            File.WriteAllText(filePath, decryptedText, Encoding.UTF8);
        }
    }
}