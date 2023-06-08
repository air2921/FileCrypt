using System.Security.Cryptography;

namespace FileCrypt
{
    internal class DecryptData : IDecryptor
    {

        private static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encryptedData, salt.Length, iv, 0, iv.Length);
                aes.IV = iv;

                using (MemoryStream encryptedStream = new MemoryStream(encryptedData, salt.Length + iv.Length, encryptedData.Length - salt.Length - iv.Length))
                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptedStream);
                    }

                    byte[] decryptedData = decryptedStream.ToArray();
                    return decryptedData;
                }
            }
        }

        public void DecryptFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = Decrypt(encryptedData, key, salt);

            using (MemoryStream decryptedStream = new MemoryStream(decryptedData))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    decryptedStream.CopyTo(fs);
                }
            }
        }
    }

    public interface IDecryptor
    {
        void DecryptFile(string filePath, byte[] key, byte[] salt);
    }
}